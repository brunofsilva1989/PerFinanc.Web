using PerFinanc.Web.Models;
using System.Globalization;

namespace PerFinanc.Web.Models.Dashboard
{
    public class DashboardViewModel
    {
        public int Ano { get; set; }
        public int Mes { get; set; }

        // Saídas (Contas Fixas)
        public decimal TotalPrevisto { get; set; }
        public decimal TotalPago { get; set; }
        public decimal TotalEmAberto => TotalPrevisto - TotalPago;

        public int QtdLancamentos { get; set; }
        public int QtdPagos { get; set; }
        public int QtdEmAberto { get; set; }
        public int QtdVencidos { get; set; }

        // Entradas
        public decimal TotalReceitas { get; set; }
        public decimal TotalFreelancers { get; set; }
        public decimal TotalEntradas => TotalReceitas + TotalFreelancers;

        // Saldo do mês
        public decimal SaldoMes => TotalEntradas - (TotalPrevisto + TotalGastosGerais);

        public int QtdReceitas { get; set; }
        public int QtdFreelancers { get; set; }

        // Total Gastos Gerais
        public decimal TotalGastosGerais { get; set; }


        // Listas pra mostrar no dash
        public List<LancamentoContaFixa> Lancamentos { get; set; } = new();
        public List<ReceitaEntrada> Receitas { get; set; } = new();
        public List<Freelance> Freelancers { get; set; } = new();

        public List<GastoGeral> GastosGerais { get; set; } = new();

        public decimal PercentPago =>
            TotalPrevisto <= 0 ? 0 : (TotalPago / TotalPrevisto) * 100m;

        public string MesNome =>
            CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat.GetMonthName(Mes);
    }
}
