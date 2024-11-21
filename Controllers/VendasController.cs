using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DsiVendas.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DsiVendas.Models;

namespace DsiVendas.Controllers
{
    public class VendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Construtor para injeção de dependência do DbContext
        public VendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listagem de Vendas
        public IActionResult Index()
        {
            var listaVendas = _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.ItensVenda)
                    .ThenInclude(i => i.Recurso) // Corrigido para incluir o objeto Recurso
                .ToList();

            return View(listaVendas);
        }

        // GET: Criação de Venda
        public IActionResult Criar()
        {
            var listaFormaPagamento = new List<SelectListItem>
            {
                new SelectListItem { Text = "Cartão de Débito", Value = "Cartão de Débito" },
                new SelectListItem { Text = "Cartão de Crédito", Value = "Cartão de Crédito" },
                new SelectListItem { Text = "Boleto", Value = "Boleto" },
                new SelectListItem { Text = "PIX", Value = "PIX" }
            };

            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            ViewBag.Recursos = new SelectList(_context.Recursos, "Id", "Nome");
            ViewBag.FormaPagamentos = listaFormaPagamento;

            return View();
        }

        // Método para obter o preço de um recurso via JSON
        [HttpGet]
        public JsonResult GetPrecoRecurso(int idRecurso)
        {
            var recurso = _context.Recursos.FirstOrDefault(p => p.Id == idRecurso);
            if (recurso != null)
            {
                return Json(recurso.Preco);
            }
            return Json(0);
        }

        // POST: Criação de Venda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Venda venda, List<ItemVenda> itensVenda)
        {
            // Adiciona a venda ao banco de dados
            _context.Add(venda);
            await _context.SaveChangesAsync();

            // Adiciona os itens da venda
            foreach (var item in itensVenda)
            {
                var recurso = _context.Recursos.Find(item.RecursoId);
                if (recurso != null)
                {
                    item.VendaId = venda.Id;
                    item.PrecoUnitario = recurso.Preco;
                    _context.ItemsVendas.Add(item); // Corrigido para ItensVendas
                }
            }

            // Salva as mudanças
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Excluir Venda
        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Confirmar Exclusão de Venda
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(int id)
        {
            var venda = await _context.Vendas
                .Include(v => v.ItensVenda)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venda == null)
            {
                return NotFound();
            }

            // Exclui os itens da venda antes de excluir a venda
            if (venda.ItensVenda != null && venda.ItensVenda.Count > 0)
            {
                _context.ItemsVendas.RemoveRange(venda.ItensVenda); // Corrigido para ItensVendas
            }

            // Exclui a venda
            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Editar Venda
        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Busca a venda incluindo cliente e itens de venda
            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.ItensVenda)
                    .ThenInclude(i => i.Recurso)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venda == null)
            {
                return NotFound();
            }

            // Popula as listas de seleção (clientes, produtos e formas de pagamento)
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", venda.ClienteId);
            ViewBag.FormaPagamentos = new List<SelectListItem>
            {
                new SelectListItem { Text = "Cartão de Débito", Value = "Cartão de Débito" },
                new SelectListItem { Text = "Cartão de Crédito", Value = "Cartão de Crédito" },
                new SelectListItem { Text = "Boleto", Value = "Boleto" },
                new SelectListItem { Text = "PIX", Value = "PIX" }
            };
            return View(venda);
        }

        // POST: Editar Venda
        [HttpPost]
        public async Task<IActionResult> Editar(int id, Venda venda, List<ItemVenda> itensVenda)
        {
            if (id != venda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualiza a venda
                    _context.Update(venda);
                    await _context.SaveChangesAsync();

                    // Remove os itens de venda antigos para substituí-los pelos novos
                    var itensAntigos = _context.ItemsVendas.Where(iv => iv.VendaId == venda.Id); // Corrigido para ItensVendas
                    _context.ItemsVendas.RemoveRange(itensAntigos); // Corrigido para ItensVendas

                    // Adiciona os itens de venda novos/atualizados
                    foreach (var item in itensVenda)
                    {
                        var recurso = _context.Recursos.Find(item.RecursoId);
                        if (recurso != null)
                        {
                            item.VendaId = venda.Id;
                            item.PrecoUnitario = recurso.Preco;
                            _context.ItemsVendas.Add(item); // Corrigido para ItensVendas
                        }
                    }

                    // Salva as mudanças
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Caso ocorra algum erro, recarrega os dados para repopular os campos
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome", venda.ClienteId);
            ViewBag.Recursos = new SelectList(_context.Recursos, "Id", "Nome");
            ViewBag.FormaPagamentos = new List<SelectListItem>
            {
                new SelectListItem { Text = "Cartão de Débito", Value = "Cartão de Débito" },
                new SelectListItem { Text = "Cartão de Crédito", Value = "Cartão de Crédito" },
                new SelectListItem { Text = "Boleto", Value = "Boleto" },
                new SelectListItem { Text = "PIX", Value = "PIX" }
            };

            return View(venda);
        }

        // Método auxiliar para verificar se a venda existe
        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.Id == id);
        }
    }
}
