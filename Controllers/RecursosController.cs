using Microsoft.AspNetCore.Mvc;
using DsiVendas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using DsiVendas.Models;

namespace DsiVendas.Controllers
{
    public class RecursosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var listaRecursos = _context.Recursos.ToList();
            return View(listaRecursos);
        }

        public IActionResult Criar()
{   
    // Popula o dropdown de tipos de recursos
    SetListaTiposRecursos();
    
    return View();
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Recurso recurso)
    {               
                // Valida a enumeração e atribui a entidade ao banco de dados
                _context.Recursos.Add(recurso);
                _context.SaveChanges();
                return RedirectToAction("Index");
            
        }
         public IActionResult Editar(int id)
    {
        var recurso = _context.Recursos.Find(id);
        if (recurso == null) return NotFound();

        SetListaTiposRecursos();
        ViewBag.IsMaquinario = recurso.Tipo == "Maquinário";
        return View(recurso);
    }

    [HttpPost]
    public IActionResult Editar(Recurso recurso)
    {
        var recursoExistente = _context.Recursos.Find(recurso.Id);
        if (recursoExistente == null) return NotFound();
        SetListaTiposRecursos();
        
        recursoExistente.Nome = recurso.Nome;
        recursoExistente.Preco = recurso.Preco;
        recursoExistente.Quantidade = recurso.Quantidade;
        recursoExistente.Tipo = recurso.Tipo;
        recursoExistente.NumeroSerie = recurso.NumeroSerie;
        recursoExistente.DataAquisicao = recurso.DataAquisicao;
        _context.Recursos.Update(recursoExistente);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
      public IActionResult Excluir(int id)
    {
        var recurso = _context.Recursos.Find(id);
        if (recurso == null) return NotFound();
        return View(recurso);
    }

    [HttpPost]
    public IActionResult Excluir(Recurso recurso)
    {
        var recursoExistente = _context.Recursos.Find(recurso.Id);
        if (recursoExistente == null) return NotFound();

        _context.Recursos.Remove(recursoExistente);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

         private void SetListaTiposRecursos()
    {
        var ListaTiposRecursos = new List<string>
        {
            "Insumo",
            "Produto",
            "Maquinário"
        };
        ViewBag.TiposRecursos = new SelectList(ListaTiposRecursos);
    }

    }
}
