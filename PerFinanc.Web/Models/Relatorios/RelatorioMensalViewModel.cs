namespace PerFinanc.Web.Models.Relatorios
{
    public class RelatorioMensalViewModel
    {
        public string Titulo { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public decimal Total { get; set; }
        public List<LinhaRelatorioDto> Linhas { get; set; } = new();
    }
}
