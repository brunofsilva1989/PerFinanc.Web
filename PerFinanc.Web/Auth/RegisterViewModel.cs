using System.ComponentModel.DataAnnotations;

namespace PerFinanc.Web.Auth
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Usuário")]
        public string Username { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare(nameof(Password), ErrorMessage = "As senhas não conferem")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
    }
}
