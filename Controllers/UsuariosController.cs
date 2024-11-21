using Microsoft.AspNetCore.Mvc;
using DsiVendas.Models; // Namespace do seu modelo
using System.Linq;
using System.Threading.Tasks;

namespace DsiVendas.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public IActionResult Index()
        {
            //var usuarios = _context.Users.ToList(); // Obtém a lista de usuários do banco
            return RedirectToAction("Register"); // Passa a lista para a View
        }
        
        // GET: Users/Register
        public IActionResult Register()
        {
            ViewData["HideLayout"] = true; // Esconde o layout
            
            return View();
        }

        // POST: Users/Register
[HttpPost ("Usuarios/Register")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Register(User user)
{
    if (ModelState.IsValid)
    {
        // Adiciona hash à senha
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash ?? string.Empty);

        // Salva o usuário no banco de dados
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Realiza o login após o registro
        HttpContext.Session.SetString("UsuarioId", user.Id.ToString());
        
        // Aqui você também pode armazenar o nome de usuário para exibição
        HttpContext.Session.SetString("UsuarioNome", user.UserName);

        // Redireciona ao HomeController
        return RedirectToAction("Index", "Home");
    }
    

    return View(user);
}
 [HttpGet]
public async Task<IActionResult> Delete(int id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
    {
        return NotFound();
    }

    return View(user);
}

[HttpPost]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
    {
        return NotFound();
    }

    _context.Users.Remove(user);
    await _context.SaveChangesAsync();

    return RedirectToAction("Index");
}

    }
}
