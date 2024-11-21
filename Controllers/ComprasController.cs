using DsiVendas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DsiVendas.Controllers
{
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ação para exibir o formulário de criação de compra
        public IActionResult Criar()
        {   
            // Carregar os dados necessários para os selects
            ViewBag.Fornecedores = new SelectList(_context.Fornecedores, "Id", "Nome");
            ViewBag.Funcionarios = new SelectList(_context.Funcionarios, "Id", "Nome");
            ViewBag.FormaPagamentos = new SelectList(_context.FormaPagamentos, "Id", "Descricao");
            ViewBag.Recursos = new SelectList(_context.Recursos, "Id", "Nome");

            // Definir a lista de formas de pagamento
            SetListaFormaPagamentos();

            return View();
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Criar(Compra compra, List<ItemCompra> itensCompra)
{
    try
    {
       
        // 1. Salvar a compra no banco de dados
            _context.Compras.Add(compra);
            await _context.SaveChangesAsync(); // Salva para obter o ID da compra
            // 2. Processar os itens da compra
            foreach (var item in itensCompra)
            {
                var recurso = await _context.Recursos.FindAsync(item.RecursoId);
                if (recurso != null)
                {
                    item.CompraId = compra.Id; // Associa o item à compra
                    item.PrecoUnitario = recurso.Preco; // Define o preço unitário do item
                    _context.ItemsCompra.Add(item); // Adiciona o item ao contexto
                }
            // 3. Salvar os itens de compra
            await _context.SaveChangesAsync();

            // 4. Redirecionar para a página de índice (lista de compras)
            return RedirectToAction(nameof(Index));
        }
    }
    catch (Exception ex)
    {
        // Log da exceção
        Console.WriteLine($"Erro ao criar compra: {ex.Message}");
        ViewBag.ErrorMessage = "Ocorreu um erro ao criar a compra. Tente novamente ou entre em contato com o suporte.";
    }

    // Recarregar as listas de seleção
    ViewBag.Fornecedores = new SelectList(_context.Fornecedores, "Id", "Nome", compra.FornecedorId);
    ViewBag.Funcionarios = new SelectList(_context.Funcionarios, "Id", "Nome", compra.FuncionarioId);
    ViewBag.FormaPagamentos = new SelectList(_context.FormaPagamentos, "Id", "Descricao", compra.FormaPagamento);
    ViewBag.Recursos = new SelectList(_context.Recursos, "Id", "Nome");
    SetListaFormaPagamentos();

    // Retornar a view com os dados da compra não salvos
    return View(compra);
}
        // Ação para listar todas as compras
        public IActionResult Index()
        {
            var compras = _context.Compras
                .Include(c => c.Fornecedor)
                .Include(c => c.Funcionario)
                .Include(c => c.ItensCompra)
                .ThenInclude(i => i.Recurso) // Incluir o recurso para exibir no item de compra
                .ToList();
    
            if (compras == null)
            {
                return NotFound();
            }

            return View(compras);
        }

        // Método para definir a lista de formas de pagamento
        private void SetListaFormaPagamentos()
        {
            var listaFormaPagamentos = new List<string>
            {
                "Pix",
                "Cartão Crédito",
                "Cartão Débito",
                "Dinheiro"
            };
            ViewBag.FormaPagamentos = new SelectList(listaFormaPagamentos);
        }

        // Ação para obter os detalhes da forma de pagamento (parcelas, etc.)
        [HttpGet]
        public async Task<IActionResult> GetDetalhesFormaPagamento(int idFormaPagamento)
        {
            var formaPagamento = await _context.FormaPagamentos
                .Where(f => f.Id == idFormaPagamento)
                .FirstOrDefaultAsync();

            if (formaPagamento == null)
            {
                return NotFound();
            }

            // Retorna as parcelas associadas à forma de pagamento, se existirem
            var parcelas = await _context.Parcelas
                .Where(p => p.FormaPagamentoId == idFormaPagamento)
                .Select(p => new 
                { 
                    p.Id, 
                    p.Descricao, 
                    p.Valor 
                })
                .ToListAsync();

            return Json(new { parcelas });
        }

        // Ação para buscar o preço de um recurso (utilizado para atualizar o preço unitário do item)
        [HttpGet]
        public async Task<IActionResult> GetPrecoRecurso(int idRecurso)
        {
            var recurso = await _context.Recursos
                .Where(r => r.Id == idRecurso)
                .FirstOrDefaultAsync();

            if (recurso == null)
            {
                return NotFound();
            }

            return Json(recurso.Preco);
        }
    }
}
