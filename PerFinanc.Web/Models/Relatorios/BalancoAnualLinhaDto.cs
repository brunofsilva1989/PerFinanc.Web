namespace PerFinanc.Web.Models.Relatorios
{
    public class BalancoAnualLinhaDto
    {
        public int Mes { get; set; }
        public decimal Receitas { get; set; }
        public decimal Despesas { get; set; }
        public decimal Saldo { get; set; }
    }
}
