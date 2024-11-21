using Microsoft.AspNetCore.Mvc;
using DsiVendas.Models;

namespace DsiVendas.Controllers
{
    public class FazendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FazendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var listaFazendas = _context.Fazendas.ToList();
            return View(listaFazendas);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Fazenda fazenda)
        {
           
                _context.Fazendas.Add(fazenda);
                _context.SaveChangesAsync();
                return RedirectToAction("Index");
            
        }


        public IActionResult Editar(int id)
        {
            var fazenda = _context.Fazendas.Find(id);
            if (fazenda == null) return NotFound();

            return View(fazenda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Fazenda fazenda)
        {
            var fazendaExistente = _context.Fazendas.Find(fazenda.Id);
            if (fazendaExistente == null) return NotFound();
            fazendaExistente.Nome = fazenda.Nome;
            fazendaExistente.Hectares = fazenda.Hectares;
            fazendaExistente.Email = fazenda.Email;
            fazendaExistente.Endereco = fazenda.Endereco;
            fazendaExistente.Telefone = fazenda.Telefone;
            _context.Fazendas.Update(fazendaExistente);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

       // Ação para exibir a página de confirmação de exclusão
    public IActionResult Remover(int id)
    {
        var fazenda = _context.Fazendas.Find(id);
        if (fazenda == null)
        {
            return NotFound();
        }
        return View(fazenda);  // Exibe a view de confirmação de remoção
    }

    // Ação POST para realmente excluir a fazenda
    [HttpPost, ActionName("Remover")]
    [ValidateAntiForgeryToken]
    public IActionResult RemoverConfirmado(int id)
    {
        var fazenda = _context.Fazendas.Find(id);
        if (fazenda == null)
        {
            return NotFound();
        }

        _context.Fazendas.Remove(fazenda);  // Remove a fazenda do banco
        _context.SaveChanges();  // Salva as alterações no banco de dados

        return RedirectToAction(nameof(Index));  // Redireciona para a página de listagem
    }
    }
}
