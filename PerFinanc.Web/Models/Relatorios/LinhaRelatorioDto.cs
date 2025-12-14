namespace PerFinanc.Web.Models.Relatorios
{
    public class LinhaRelatorioDto
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
    }
}
