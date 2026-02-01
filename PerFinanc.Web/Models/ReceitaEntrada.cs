using System.ComponentModel.DataAnnotations;

namespace PerFinanc.Web.Models
{
    public class ReceitaEntrada
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }

        [Display(Name = "Valor")]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Display(Name = "Data de Recebimento")]
        [DataType(DataType.Date)]
        public DateTime DataRecebimento { get; set; }

        [StringLength(50)]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; }        
    }
}
