using System.ComponentModel.DataAnnotations;

namespace PerFinanc.Web.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o usuário ou o e-mail")]
        [Display(Name = "Usuário ou e-mail")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar-me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
