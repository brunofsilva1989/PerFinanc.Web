namespace PerFinanc.Web.Models
{
    public class CriarUsuarioViewModel
    {

        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool LockoutEnabled { get; set; }
        public bool LockoutDisabled { get; set; }
        public List<string> Roles { get; set; }
        public List<string> SelectedRoles { get; set; } = new List<string>();

    }
}
