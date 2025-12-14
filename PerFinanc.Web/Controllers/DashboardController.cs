using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Data;
using PerFinanc.Web.Models.Dashboard;
using System.Security.Claims;

namespace PerFinanc.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly PerFinancDbContext _context;

        public DashboardController(PerFinancDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? ano, int? mes)
        {
            var hoje = DateTime.Today;
            var anoRef = ano ?? hoje.Year;
            var mesRef = mes ?? hoje.Month;

            // Saídas: Contas Fixas do mês
            var lancamentos = _context.LancamentoContaFixa
                .AsNoTracking()
                .Include(l => l.ContaFixa)
                .Where(l => l.Ano == anoRef && l.Mes == mesRef)
                .ToList();

            var totalPrevisto = lancamentos.Sum(l => l.ValorPrevisto);
            var totalPago = lancamentos.Sum(l => l.ValorPago ?? 0m);

            var qtdPagos = lancamentos.Count(l => l.EstaPago);
            var qtdEmAberto = lancamentos.Count(l => !l.EstaPago);

            var qtdVencidos = lancamentos.Count(l =>
                !l.EstaPago && l.DataVencimento.Date < hoje);

            // Entradas: Receitas do mês (pela DATA)
            var receitas = _context.ReceitaEntrada
                .AsNoTracking()
                .Where(r => r.DataRecebimento.Year == anoRef && r.DataRecebimento.Month == mesRef)
                .OrderByDescending(r => r.DataRecebimento)
                .ToList();

            var totalReceitas = receitas.Sum(r => r.Valor);

            // Entradas: Freelancers do mês (pela DATA)
            var freelancers = _context.Freelance
                .AsNoTracking()
                .Where(f => f.DataRecebimento.Year == anoRef && f.DataRecebimento.Month == mesRef)
                .OrderByDescending(f => f.DataRecebimento)
                .ToList();

            var totalFreelancers = freelancers.Sum(f => f.Valor);

            // Gastos Gerais do mês (pela DATA)
            var gastosGerais = _context.GastoGeral
                .AsNoTracking()
                .Where(g => g.DataGasto.Year == anoRef && g.DataGasto.Month == mesRef)
                .OrderByDescending(g => g.DataGasto)
                .ToList();

            var totalGastosGerais = gastosGerais.Sum(g => g.Valor);

            var vm = new DashboardViewModel
            {
                Ano = anoRef,
                Mes = mesRef,

                Lancamentos = lancamentos,
                TotalPrevisto = totalPrevisto,
                TotalPago = totalPago,
                QtdLancamentos = lancamentos.Count,
                QtdPagos = qtdPagos,
                QtdEmAberto = qtdEmAberto,
                QtdVencidos = qtdVencidos,

                Receitas = receitas,
                TotalReceitas = totalReceitas,
                QtdReceitas = receitas.Count,

                Freelancers = freelancers,
                TotalFreelancers = totalFreelancers,
                QtdFreelancers = freelancers.Count,

                GastosGerais = gastosGerais,
                TotalGastosGerais = totalGastosGerais
            };


            return View(vm);
        }
    }
}
