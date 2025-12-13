using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Data;

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

            var lancamentos = _context.LancamentoContaFixa
                .Include(l => l.ContaFixa)
                .Where(l => l.Ano == anoRef && l.Mes == mesRef)
                .ToList();

            var totalPrevisto = lancamentos.Sum(l => l.ValorPrevisto);
            var totalPago = lancamentos.Sum(l => l.ValorPago ?? 0);

            ViewBag.Ano = anoRef;
            ViewBag.Mes = mesRef;
            ViewBag.TotalPrevisto = totalPrevisto;
            ViewBag.TotalPago = totalPago;

            return View(lancamentos);
        }
    }
}
