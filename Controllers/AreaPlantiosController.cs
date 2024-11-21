using DsiVendas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DsiVendas.Controllers
{
public class AreaPlantiosController : Controller
{
    private readonly ApplicationDbContext context;

    public AreaPlantiosController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        var listaAreas = context.AreaPlantios
            .Include(f => f.Fazenda)
            .ToList();
        return View(listaAreas);
    }

    public IActionResult Criar()
    {
        ViewBag.Fazendas = context.Fazendas.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Criar(AreaPlantio areaPlantio)
    {
    ViewBag.Fazendas = new SelectList(context.Fazendas, "Id", "Nome");
        context.AreaPlantios.Add(areaPlantio);
        context.SaveChanges();

        return RedirectToAction("Index");
    }


    public IActionResult Editar(int id)
{
    var areaPlantio = context.AreaPlantios.Find(id);
    if (areaPlantio == null) return NotFound();

    // Converte as Fazendas para SelectListItem para o dropdown
    ViewBag.Fazendas = new SelectList(context.Fazendas, "Id", "Nome");

    return View(areaPlantio);
}
    [HttpPost]
public IActionResult Editar(AreaPlantio areaPlantio)
{
    var areaPlantioExistente = context.AreaPlantios.Find(areaPlantio.Id);
    if (areaPlantioExistente == null) return NotFound();

    // Converte as Fazendas para SelectListItem para o dropdown novamente
    ViewBag.Fazendas = new SelectList(context.Fazendas, "Id", "Nome");

    // Atualiza os dados da área de plantio
    areaPlantioExistente.Nome = areaPlantio.Nome;
    areaPlantioExistente.Hectares = areaPlantio.Hectares;
    areaPlantioExistente.Localizacao = areaPlantio.Localizacao;
    areaPlantioExistente.Descricao = areaPlantio.Descricao;
    areaPlantioExistente.FazendaId = areaPlantio.FazendaId; // Presumo que o "FazendaId" é a chave estrangeira

    context.AreaPlantios.Update(areaPlantioExistente);
    context.SaveChanges();

    return RedirectToAction("Index");
}
    [HttpGet]
    public IActionResult Remover(int id)
    {
        var areaPlantio = context.AreaPlantios.Include(a => a.Fazenda).FirstOrDefault(a => a.Id == id);
        if (areaPlantio == null)
        {
            return NotFound();
        }

        return View(areaPlantio);
    }

    // Método para processar a remoção da área de plantio
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Excluir(int id)
    {
        var areaPlantio = context.AreaPlantios.FirstOrDefault(a => a.Id == id);
        if (areaPlantio == null)
        {
            return NotFound();
        }

        context.AreaPlantios.Remove(areaPlantio);
        context.SaveChanges();

        TempData["SuccessMessage"] = "Área de plantio removida com sucesso!";
        return RedirectToAction("Index");
    }
}

}
