using PerFinanc.Web.Auth;
using System.ComponentModel.DataAnnotations;

namespace PerFinanc.Web.Models
{
    public class ContaFixa
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;  // Ex.: "ALUGUEL"

        [Display(Name = "Dia de Vencimento")]
        [Range(1, 31)]
        public int DiaVencimento { get; set; }           // Ex.: 6, 10, 20 etc.

        [Display(Name = "Valor Padrão")]
        [Range(0, double.MaxValue)]
        public decimal ValorPadrao { get; set; }         // Ex.: 723,00

        [Display(Name = "Já vem descontado")]
        public bool JaVemDescontado { get; set; }        // Para os "Já vem descontado" da planilha

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;        

        // Navegação
        public ICollection<LancamentoContaFixa> Lancamentos { get; set; }
            = new List<LancamentoContaFixa>();

    }
}
