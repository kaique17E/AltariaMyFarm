using Microsoft.AspNetCore.Mvc;
using DsiVendas.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DsiVendas.Controllers
{
    public class FuncionariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FuncionariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Exibe a lista de funcionários
        public IActionResult Index()
        {
            var funcionarios = _context.Funcionarios.ToList();
            return View(funcionarios);
        }

        // Exibe o formulário de cadastro de funcionário
      public IActionResult Criar()
{
    // Carregar a lista de fazendas e convertê-las em SelectListItems para o dropdown
    var fazendas = _context.Fazendas.ToList();

    // Converte a lista de fazendas em uma lista de SelectListItem
    ViewBag.Fazendas = new SelectList(fazendas, "Id", "Nome"); // "Id" é o valor e "Nome" é o texto exibido no dropdown

    return View();
}
        // Processa o cadastro do funcionário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Funcionario funcionario)
        {
            // Verifica se o modelo é válido antes de salvar
            if (ModelState.IsValid)
            {
                // Adiciona o funcionário ao banco de dados
                _context.Funcionarios.Add(funcionario);
                _context.SaveChanges();
                return RedirectToAction("Index"); // Redireciona para a lista de funcionários
            }

            // Se o modelo não for válido, recarrega a lista de fazendas e a view de criação
            ViewBag.Fazendas = _context.Fazendas.ToList();
            return View(funcionario);
        }
        [HttpGet]
public async Task<IActionResult> Editar(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    // Busca o funcionário pelo ID
    var funcionario = await _context.Funcionarios.FindAsync(id);

    if (funcionario == null)
    {
        return NotFound();
    }

    // Carregar as fazendas para o dropdown
    ViewBag.Fazendas = new SelectList(_context.Fazendas, "Id", "Nome", funcionario.FazendaId);

    return View(funcionario);
}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Editar(Funcionario funcionario)
{
    if (!ModelState.IsValid)
    {
        // Recarregar o ViewBag.Fazendas para o dropdown
        ViewBag.Fazendas = new SelectList(_context.Fazendas, "Id", "Nome", funcionario.FazendaId);
        return View(funcionario);
    }

    _context.Update(funcionario);
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}


    }
}
