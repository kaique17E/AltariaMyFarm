namespace DsiVendas.Models;
public class AreaPlantio{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public decimal Hectares { get; set; }
    public string? Localizacao { get; set; }
    public string? Descricao { get; set; }
    public required int FazendaId { get; set; }	
    public Fazenda? Fazenda { get; set; }
}
