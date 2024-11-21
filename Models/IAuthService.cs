using DsiVendas.Models;

namespace DsiVendas.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}