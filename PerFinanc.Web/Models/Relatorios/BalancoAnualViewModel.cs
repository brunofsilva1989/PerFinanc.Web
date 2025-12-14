namespace PerFinanc.Web.Models.Relatorios
{
    public class BalancoAnualViewModel
    {
        public int Ano { get; set; }
        public List<BalancoAnualLinhaDto> Linhas { get; set; } = new();
    }
}
