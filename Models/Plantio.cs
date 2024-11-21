using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DsiVendas.Models
{
    public class Plantio
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public int AreaPlantioId { get; set; }
        [JsonIgnore] [ValidateNever] public AreaPlantio? AreaPlantio { get; set; } 
        public ICollection<ItemRecurso> ItensRecurso { get; set; }
    }
}
