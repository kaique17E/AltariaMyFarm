using DsiVendas.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DsiVendas.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            // Busca o usuário no banco de dados
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);
            
            if (user == null)
            {
                return null; // Usuário não encontrado
            }

            // Verifica se a senha fornecida corresponde ao hash da senha armazenado
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null; // Senha incorreta
            }

            return user; // Usuário autenticado com sucesso
        }

        // Método para gerar um hash seguro da senha
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
