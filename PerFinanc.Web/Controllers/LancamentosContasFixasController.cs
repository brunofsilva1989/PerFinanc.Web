using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Data;
using PerFinanc.Web.Models;

namespace PerFinanc.Web.Controllers
{
    [Authorize]
    public class LancamentosContasFixasController : Controller
    {
        private readonly PerFinancDbContext _context;

        public LancamentosContasFixasController(PerFinancDbContext context)
        {
            _context = context;
        }

        // GET: LancamentosContasFixas
        public async Task<IActionResult> Index()
        {
            var perFinancDbContext = _context.LancamentoContaFixa.Include(l => l.ContaFixa);
            return View(await perFinancDbContext.ToListAsync());
        }

        // GET: LancamentosContasFixas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lancamentoContaFixa = await _context.LancamentoContaFixa
                .Include(l => l.ContaFixa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lancamentoContaFixa == null)
            {
                return NotFound();
            }

            return View(lancamentoContaFixa);
        }

        // GET: LancamentosContasFixas/Create
        public IActionResult Create()
        {
            ViewData["ContaFixaId"] = new SelectList(_context.ContaFixa, "Id", "Nome");
            return View();
        }
        
        // POST: LancamentosContasFixas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContaFixaId,Ano,Mes,ValorPago,DataPagamento,Observacao")] LancamentoContaFixa lancamentoContaFixa)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ContaFixaId"] = new SelectList(_context.ContaFixa, "Id", "Nome", lancamentoContaFixa.ContaFixaId);
                return View(lancamentoContaFixa);
            }

            // 1) Busca a ContaFixa pra pegar DiaVencimento e ValorPadrao
            var conta = await _context.ContaFixa.FindAsync(lancamentoContaFixa.ContaFixaId);
            if (conta == null)
            {
                ModelState.AddModelError(nameof(lancamentoContaFixa.ContaFixaId), "Conta fixa não encontrada.");
                ViewData["ContaFixaId"] = new SelectList(_context.ContaFixa, "Id", "Nome", lancamentoContaFixa.ContaFixaId);
                return View(lancamentoContaFixa);
            }

            // 2) Define ValorPrevisto baseado na ContaFixa (você pode permitir editar isso depois, se quiser)
            lancamentoContaFixa.ValorPrevisto = conta.ValorPadrao;

            // 3) Calcula DataVencimento baseado no Ano/Mes e DiaVencimento da ContaFixa
            lancamentoContaFixa.DataVencimento = CalcularVencimento(lancamentoContaFixa.Ano, lancamentoContaFixa.Mes, conta.DiaVencimento);

            // 4) Salva
            _context.Add(lancamentoContaFixa);
            TempData["Mensagem"] = "Registro criado com sucesso!";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: LancamentosContasFixas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lancamentoContaFixa = await _context.LancamentoContaFixa.FindAsync(id);
            if (lancamentoContaFixa == null)
            {
                return NotFound();
            }
            ViewData["ContaFixaId"] = new SelectList(_context.ContaFixa, "Id", "Nome", lancamentoContaFixa.ContaFixaId);
            return View(lancamentoContaFixa);
        }

        // POST: LancamentosContasFixas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContaFixaId,Ano,Mes,ValorPrevisto,ValorPago,DataPagamento,Observacao")] LancamentoContaFixa lancamentoContaFixa)
        {
            if (id != lancamentoContaFixa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lancamentoContaFixa);
                    TempData["Mensagem"] = "Registro atualizado com sucesso!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LancamentoContaFixaExists(lancamentoContaFixa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContaFixaId"] = new SelectList(_context.ContaFixa, "Id", "Nome", lancamentoContaFixa.ContaFixaId);
            return View(lancamentoContaFixa);
        }

        // GET: LancamentosContasFixas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lancamentoContaFixa = await _context.LancamentoContaFixa
                .Include(l => l.ContaFixa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lancamentoContaFixa == null)
            {
                return NotFound();
            }

            return View(lancamentoContaFixa);
        }

        // POST: LancamentosContasFixas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lancamentoContaFixa = await _context.LancamentoContaFixa.FindAsync(id);
            if (lancamentoContaFixa != null)
            {
                _context.LancamentoContaFixa.Remove(lancamentoContaFixa);
            }

            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "Registro excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool LancamentoContaFixaExists(int id)
        {
            return _context.LancamentoContaFixa.Any(e => e.Id == id);
        }

        public static DateTime CalcularVencimento(int ano, int mes, int diaVencimento)
        {
            var ultimoDia = DateTime.DaysInMonth(ano, mes);
            var dia = Math.Min(diaVencimento, ultimoDia);
            return new DateTime(ano, mes, dia);
        }
    }
}
