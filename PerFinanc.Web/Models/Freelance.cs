using System.ComponentModel.DataAnnotations;

namespace PerFinanc.Web.Models
{
    public class Freelance
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

        [Display(Name = "Ano")]
        [DataType(DataType.Date)]
        public int Ano { get; set; }

        [Display(Name = "Mês")]
        [DataType(DataType.Date)]
        public int Mes { get; set; }
    }
}
