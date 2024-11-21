namespace DsiVendas.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataVenda { get; set; }
        public string? FormaPagamento { get; set; }
        public ICollection<ItemVenda> ItensVenda { get; set; } // Manter apenas uma definição
        public decimal Total => ItensVenda?.Where(item => item?.SubTotal != null).Sum(item => item.SubTotal) ?? 0;
        public Cliente? Cliente { get; set; }
    }
}
