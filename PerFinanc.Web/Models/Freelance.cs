using System.ComponentModel.DataAnnotations;

namespace PerFinanc.Web.Models
{
    public class Freelance
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

        [Display(Name = "Data de Recebimento")]
        [DataType(DataType.Date)]
        public DateTime DataRecebimento { get; set; }

        [StringLength(50)]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; } = string.Empty;        
    }
}
