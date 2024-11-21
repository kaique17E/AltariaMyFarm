using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DsiVendas.Models;
using Microsoft.EntityFrameworkCore;

namespace DsiVendas.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("UserName and password are required");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials");
            }

            // Armazena a informação do usuário na sessão
            HttpContext.Session.SetString("UsuarioId", user.Id.ToString());
            return Ok(new { Message = "Login successful" });
        }

        // Logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            // Limpa a sessão do usuário
            HttpContext.Session.Remove("UsuarioId");

            // Redireciona para a página de registro
            return RedirectToAction("Index", "Usuarios");
        }

    }
}
