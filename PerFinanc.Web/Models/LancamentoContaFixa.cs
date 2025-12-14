using System.ComponentModel.DataAnnotations;

namespace PerFinanc.Web.Models
{
    public class LancamentoContaFixa : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int ContaFixaId { get; set; }
        public ContaFixa? ContaFixa { get; set; }

        [Range(2000, 2100)]
        public int Ano { get; set; }

        [Range(1, 12)]
        public int Mes { get; set; }

        [Display(Name = "Data de Vencimento")]
        [DataType(DataType.Date)]
        public DateTime DataVencimento { get; set; }

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
            ValorPago.HasValue && ValorPrevisto > 0 && ValorPago.Value >= ValorPrevisto;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Se informou ValorPago, exige DataPagamento
            if (ValorPago.HasValue && ValorPago.Value > 0 && !DataPagamento.HasValue)
            {
                yield return new ValidationResult(
                    "Informe a data de pagamento quando houver valor pago.",
                    new[] { nameof(DataPagamento) }
                );
            }

            // Opcional: se informou DataPagamento, exige ValorPago
            if (DataPagamento.HasValue && (!ValorPago.HasValue || ValorPago.Value <= 0))
            {
                yield return new ValidationResult(
                    "Informe o valor pago quando houver data de pagamento.",
                    new[] { nameof(ValorPago) }
                );
            }
        }
    }
}
