using Microsoft.AspNetCore.Mvc;
using DsiVendas.Models;
using System.Linq;
using System.Collections.Generic;
using DsiVendas.Models;

namespace DsiVendas.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly ApplicationDbContext context;

        public FornecedoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var listaFornecedores = context.Fornecedores.ToList();
            return View(listaFornecedores);
        }

        [Produces("application/json")]
        public JsonResult Api()
        {
            var listaFornecedor = context.Fornecedores.ToList();
            return Json(listaFornecedor);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                context.Fornecedores.Add(fornecedor);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fornecedor);
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var fornecedor = context.Fornecedores.Find(id);
            if (fornecedor == null)
            {
                return NotFound();
            }
            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                var fornecedorExistente = context.Fornecedores.Find(fornecedor.Id);
                if (fornecedorExistente == null)
                {
                    return NotFound();
                }
                fornecedorExistente.CNPJ = fornecedor.CNPJ;
                fornecedorExistente.Nome = fornecedor.Nome;
                fornecedorExistente.Cidade = fornecedor.Cidade;
                fornecedorExistente.Telefone = fornecedor.Telefone;
                context.Fornecedores.Update(fornecedorExistente);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            var fornecedor = context.Fornecedores.Find(id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            context.Fornecedores.Remove(fornecedor);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
