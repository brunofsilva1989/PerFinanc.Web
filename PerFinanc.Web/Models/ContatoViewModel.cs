using System.ComponentModel.DataAnnotations;

namespace PerFinanc.Web.Models
{
    public class ContatoViewModel
    {
        [Required(ErrorMessage = "Informe seu nome.")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe seu e-mail.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [StringLength(150)]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o assunto.")]
        [StringLength(120, ErrorMessage = "Assunto deve ter no máximo 120 caracteres.")]
        [Display(Name = "Assunto")]
        public string Assunto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Escreva sua mensagem.")]
        [StringLength(2000, ErrorMessage = "Mensagem deve ter no máximo 2000 caracteres.")]
        [Display(Name = "Mensagem")]
        public string Mensagem { get; set; } = string.Empty;
    }
}
