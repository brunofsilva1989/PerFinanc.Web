using System.ComponentModel.DataAnnotations;

namespace PerFinanc.Web.Models
{
    public class GastoGeral
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        [Display(Name = "Valor")]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Display(Name = "Data do Gasto")]
        [DataType(DataType.Date)]
        public DateTime DataGasto { get; set; }

        [StringLength(50)]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; } = string.Empty;

        [Range(2000, 2100)]
        public int Ano { get; set; }

        [Range(1, 12)]
        public int Mes { get; set; }

    }
}
