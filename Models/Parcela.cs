namespace DsiVendas.Models;

public class Parcela
{
    public int Id { get; set; }
    public int FormaPagamentoId { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
}
