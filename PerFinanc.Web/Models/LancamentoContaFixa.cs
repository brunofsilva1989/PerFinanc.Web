using System.ComponentModel.DataAnnotations;

namespace PerFinanc.Web.Models
{
    public class LancamentoContaFixa
    {
        public int Id { get; set; }

        [Required]
        public int ContaFixaId { get; set; }
        public ContaFixa ContaFixa { get; set; }

        [Range(2000, 2100)]
        public int Ano { get; set; }           // 2025

        [Range(1, 12)]
        public int Mes { get; set; }           // 1 = Janeiro, 2 = Fevereiro...

        [Display(Name = "Valor Previsto")]
        [Range(0, double.MaxValue)]
        public decimal ValorPrevisto { get; set; }

        [Display(Name = "Valor Pago")]
        [Range(0, double.MaxValue)]
        public decimal? ValorPago { get; set; }

        [Display(Name = "Data de Pagamento")]
        public DateTime? DataPagamento { get; set; }

        [Display(Name = "Observação")]
        [StringLength(200)]
        public string? Observacao { get; set; }

        public bool EstaPago =>
            ValorPago.HasValue && ValorPago.Value >= ValorPrevisto && ValorPrevisto > 0;
    }
}
