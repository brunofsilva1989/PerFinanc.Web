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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContaFixaId,Ano,Mes,ValorPrevisto,ValorPago,DataPagamento,Observacao")] LancamentoContaFixa lancamentoContaFixa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lancamentoContaFixa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContaFixaId"] = new SelectList(_context.ContaFixa, "Id", "Nome", lancamentoContaFixa.ContaFixaId);
            return View(lancamentoContaFixa);
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
            return RedirectToAction(nameof(Index));
        }

        private bool LancamentoContaFixaExists(int id)
        {
            return _context.LancamentoContaFixa.Any(e => e.Id == id);
        }
    }
}
