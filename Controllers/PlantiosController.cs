using Microsoft.AspNetCore.Mvc;
using DsiVendas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DsiVendas.Controllers
{
    public class PlantiosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlantiosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action para exibir a lista de plantios
        public async Task<IActionResult> Index()
        {
            var plantios = await _context.Plantios
                .Include(p => p.AreaPlantio)
                .ToListAsync();
            return View(plantios);
        }

        // Action para criar um novo Plantio
        public IActionResult Criar()
        {   
            CarregarItensRecurso();
            return View();
        }
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Criar(Plantio plantio)
{
    try
    {

            ViewBag.AreasPlantio = new SelectList(_context.AreaPlantios, "Id", "Nome");
            // Salva o plantio no banco de dados
            _context.Plantios.Add(plantio);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Redireciona para a lista de plantios
    }
    catch (Exception ex)
    {
        // Log da exceção
        Console.WriteLine($"Erro ao criar plantio: {ex.Message}");
        ViewBag.ErrorMessage = "Ocorreu um erro ao criar o plantio. Tente novamente ou entre em contato com o suporte.";
    }

    ViewBag.AreasPlantio = new SelectList(_context.AreaPlantios, "Id", "Nome");
    CarregarItensRecurso(); // Recarrega itens de recurso

    return View(plantio); // Retorna à view com os dados existentes
}
    private void CarregarItensRecurso()
{
    ViewBag.Recursos = new SelectList(_context.Recursos, "Id", "Nome");

    // Lista de nomes dos recursos de maquinário
    ViewBag.RecursosMaquinarioNome = new SelectList(
        _context.Recursos.Where(r => r.Tipo == "Maquinário")
            .Select(r => new { r.Id, r.Nome })
            .ToList(),
        "Id",
        "Nome"
    );

    // Lista de números de série dos recursos de maquinário
    ViewBag.RecursosMaquinarioSerie = new SelectList(
        _context.Recursos.Where(r => r.Tipo == "Maquinário")
            .Select(r => new { r.Id, r.NumeroSerie })
            .ToList(),
        "Id",
        "NumeroSerie"
    );

    // Define a lista de insumos sem duplicação
    ViewBag.RecursosInsumos = new SelectList(
        _context.Recursos.Where(r => r.Tipo == "Insumo"),
        "Id",
        "Nome"
    );

    ViewBag.RecursosProdutos = new SelectList(
        _context.Recursos.Where(r => r.Tipo == "Produto"),
        "Id",
        "Nome"
    );
    
    ViewBag.AreasPlantio = new SelectList(_context.AreaPlantios, "Id", "Nome");
}
        [HttpGet]
        public async Task<IActionResult> GetPrecoRecurso(int idRecurso)
        {
            var recurso = await _context.Recursos.FindAsync(idRecurso);
            if (recurso == null) return NotFound();
            return Json(new { preco = recurso.Preco, quantidadeDisponivel = recurso.Quantidade });
        }

        [HttpGet]
        public async Task<IActionResult> GetSerieRecurso(int idRecurso)
        {
            var recurso = await _context.Recursos.FindAsync(idRecurso);
            if (recurso == null) return NotFound();
            return Json(new { numeroSerie = recurso.NumeroSerie });
        }
    }
}