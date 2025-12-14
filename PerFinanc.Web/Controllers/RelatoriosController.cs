using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Data; // ajuste namespace do seu DbContext
using PerFinanc.Web.Models.Relatorios;
using Rotativa.AspNetCore;
using System.Security.Claims;

namespace PerFinanc.Web.Controllers
{
    [Authorize]
    public class RelatoriosController : Controller
    {
        private readonly PerFinancDbContext _db;

        public RelatoriosController(PerFinancDbContext db)
        {
            _db = db;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        public IActionResult Index() => View();

        // /Relatorios/DespesasMensais?ano=2025&mes=12
        public async Task<IActionResult> DespesasMensais(int? ano, int? mes)
        {
            var userId = GetUserId();
            var hoje = DateTime.Today;

            int a = ano ?? hoje.Year;
            int m = mes ?? hoje.Month;

            var inicio = new DateTime(a, m, 1);
            var fim = inicio.AddMonths(1);

            // 1) Gastos gerais
            var gastosGerais = await _db.GastoGeral
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.DataGasto >= inicio && x.DataGasto < fim)
                .Select(x => new LinhaRelatorioDto
                {
                    Data = x.DataGasto,
                    Descricao = x.Descricao,
                    Categoria = x.Categoria ?? "Sem categoria",
                    Tipo = "Gasto Geral",
                    Valor = x.Valor
                })
                .ToListAsync();

            // 2) Lançamentos de contas fixas (pelos vencimentos do mês)
            var contasFixas = await _db.LancamentoContaFixa
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.DataVencimento >= inicio && x.DataVencimento < fim)
                .Select(x => new LinhaRelatorioDto
                {
                    Data = x.DataVencimento,
                    Descricao = x.Observacao ?? "(Sem Observação)",                 // ajuste se o campo for diferente
                    Categoria = "Conta Fixa",
                    Tipo = "Conta Fixa",
                    Valor = x.ValorPago ?? x.ValorPrevisto                    // ajuste se o campo for diferente
                })
                .ToListAsync();

            var linhas = gastosGerais
                .Concat(contasFixas)
                .OrderByDescending(x => x.Data)
                .ToList();

            var total = linhas.Sum(x => x.Valor);

            var vm = new RelatorioMensalViewModel
            {
                Ano = a,
                Mes = m,
                Titulo = "Relatório de Despesas Mensais",
                Total = total,
                Linhas = linhas
            };

            return View(vm);
        }

        // /Relatorios/ReceitasMensais?ano=2025&mes=12
        public async Task<IActionResult> ReceitasMensais(int? ano, int? mes)
        {
            var userId = GetUserId();
            var hoje = DateTime.Today;

            int a = ano ?? hoje.Year;
            int m = mes ?? hoje.Month;

            var inicio = new DateTime(a, m, 1);
            var fim = inicio.AddMonths(1);

            // Exemplo: Freelance como receitas
            var receitas = await _db.Freelance
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.DataRecebimento >= inicio && x.DataRecebimento < fim)
                .Select(x => new LinhaRelatorioDto
                {
                    Data = x.DataRecebimento,
                    Descricao = x.Descricao,
                    Categoria = x.Categoria ?? "Sem categoria",
                    Tipo = "Freelance",
                    Valor = x.Valor
                })
                .OrderByDescending(x => x.Data)
                .ToListAsync();

            var vm = new RelatorioMensalViewModel
            {
                Ano = a,
                Mes = m,
                Titulo = "Relatório de Receitas Mensais",
                Total = receitas.Sum(x => x.Valor),
                Linhas = receitas
            };

            return View(vm);
        }

        // /Relatorios/BalancoAnual?ano=2025
        public async Task<IActionResult> BalancoAnual(int? ano)
        {
            var userId = GetUserId();
            var hoje = DateTime.Today;

            int a = ano ?? hoje.Year;
            var inicioAno = new DateTime(a, 1, 1);
            var fimAno = inicioAno.AddYears(1);

            // Despesas do ano (GastoGeral + LancamentoContaFixa)
            var despesasGastoGeral = await _db.GastoGeral
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.DataGasto >= inicioAno && x.DataGasto < fimAno)
                .GroupBy(x => x.DataGasto.Month)
                .Select(g => new { Mes = g.Key, Total = g.Sum(x => x.Valor) })
                .ToListAsync();

            var despesasContaFixa = await _db.LancamentoContaFixa
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.DataVencimento >= inicioAno && x.DataVencimento < fimAno)
                .GroupBy(x => x.DataVencimento.Month)
                .Select(g => new { Mes = g.Key, Total = g.Sum(x => x.ValorPago) })
                .ToListAsync();

            // Receitas do ano (Freelance)
            var receitasFreelance = await _db.Freelance
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.DataRecebimento >= inicioAno && x.DataRecebimento < fimAno)
                .GroupBy(x => x.DataRecebimento.Month)
                .Select(g => new { Mes = g.Key, Total = g.Sum(x => x.Valor) })
                .ToListAsync();

            var linhas = new List<BalancoAnualLinhaDto>();

            for (int mes = 1; mes <= 12; mes++)
            {
                var desp1 = despesasGastoGeral.FirstOrDefault(x => x.Mes == mes)?.Total ?? 0m;
                var desp2 = despesasContaFixa.FirstOrDefault(x => x.Mes == mes)?.Total ?? 0m;
                var rec = receitasFreelance.FirstOrDefault(x => x.Mes == mes)?.Total ?? 0m;

                var despTotal = desp1 + desp2;
                linhas.Add(new BalancoAnualLinhaDto
                {
                    Mes = mes,
                    Receitas = rec,
                    Despesas = despTotal,
                    Saldo = rec - despTotal
                });
            }

            var vm = new BalancoAnualViewModel
            {
                Ano = a,
                Linhas = linhas
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> DespesasMensaisPdf(int? ano, int? mes)
        {
            var result = await DespesasMensais(ano, mes) as ViewResult;
            var vm = result?.Model as RelatorioMensalViewModel;

            if (vm == null) return NotFound();

            return new ViewAsPdf("DespesasMensaisPdf", vm)
            {
                FileName = $"Despesas_{vm.Ano}_{vm.Mes:00}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--print-media-type --disable-smart-shrinking --margin-top 12 --margin-right 12 --margin-bottom 12 --margin-left 12"
            };
        }

        [HttpGet]
        public async Task<IActionResult> ReceitasMensaisPdf(int? ano, int? mes)
        {
            var result = await ReceitasMensais(ano, mes) as ViewResult;
            var vm = result?.Model as RelatorioMensalViewModel;
            if (vm == null) return NotFound();
            return new ViewAsPdf("ReceitasMensaisPdf", vm)
            {
                FileName = $"Receitas_{vm.Ano}_{vm.Mes:00}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "-print-media-type --disable-smart-shrinking --margin-top 12 --margin-right 12 --margin-bottom 12 --margin-left 12"
            };
        }

        [HttpGet]
        public async Task<IActionResult> BalancoAnualPdf(int? ano)
        {
            var result = await BalancoAnual(ano) as ViewResult;
            var vm = result?.Model as BalancoAnualViewModel;

            if (vm == null) return NotFound();
            return new ViewAsPdf("BalancoAnualPdf", vm)
            {
                FileName = $"Balanco_{vm.Ano}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "-print-media-type --disable-smart-shrinking --margin-top 12 --margin-right 12 --margin-bottom 12 --margin-left 12"
            };
        }
    }
}
