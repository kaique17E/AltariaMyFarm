namespace DsiVendas.Models;
public class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? PasswordHash { get; set; } // Aqui você pode armazenar a senha de forma criptografada
}
