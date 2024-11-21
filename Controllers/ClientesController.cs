using Microsoft.AspNetCore.Mvc;
using DsiVendas.Models;
using System.Collections.Generic;
using System.Linq;
using DsiVendas.Models;

namespace DsiVendas.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext context;

        public ClientesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var listaClientes = context.Clientes.ToList();
            return View(listaClientes);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Cliente cliente)
        {
            context.Clientes.Add(cliente);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            var cliente = context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Cliente cliente)
        {
            var clienteExistente = context.Clientes.Find(cliente.Id);
            if (clienteExistente == null)
            {
                return NotFound();
            }
            clienteExistente.Nome = cliente.Nome;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Telefone = cliente.Telefone;
            context.Clientes.Update(clienteExistente);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            var cliente = context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            context.Clientes.Remove(cliente);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
