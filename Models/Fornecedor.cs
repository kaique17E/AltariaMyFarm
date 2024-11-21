namespace DsiVendas.Models;

public class Fornecedor
{
  public int Id { get; set; }
  public string CNPJ { get; set; } = string.Empty;
  public string? Nome { get; set; } = string.Empty;
  public string Cidade { get; set; } = string.Empty;
  public string Telefone { get; set; } = string.Empty;
}
