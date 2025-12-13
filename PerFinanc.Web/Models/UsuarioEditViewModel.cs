namespace PerFinanc.Web.Models
{
    public class UsuarioEditViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public List<string> AllRoles { get; set; } = new();
        public string SelectedRole { get; set; }

        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
