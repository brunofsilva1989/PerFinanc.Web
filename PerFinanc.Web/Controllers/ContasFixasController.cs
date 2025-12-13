using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Data;
using PerFinanc.Web.Models;

namespace PerFinanc.Web.Controllers
{
    [Authorize]
    public class ContasFixasController : Controller
    {
        private readonly PerFinancDbContext _context;

        public ContasFixasController(PerFinancDbContext context)
        {
            _context = context;
        }

        // GET: ContaFixas
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContaFixa.ToListAsync());
        }

        // GET: ContaFixas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaFixa = await _context.ContaFixa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contaFixa == null)
            {
                return NotFound();
            }

            return View(contaFixa);
        }

        // GET: ContaFixas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContaFixas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DiaVencimento,ValorPadrao,JaVemDescontado,Ativo")] ContaFixa contaFixa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contaFixa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contaFixa);
        }

        // GET: ContaFixas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaFixa = await _context.ContaFixa.FindAsync(id);
            if (contaFixa == null)
            {
                return NotFound();
            }
            return View(contaFixa);
        }

        // POST: ContaFixas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DiaVencimento,ValorPadrao,JaVemDescontado,Ativo")] ContaFixa contaFixa)
        {
            if (id != contaFixa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contaFixa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContaFixaExists(contaFixa.Id))
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
            return View(contaFixa);
        }

        // GET: ContaFixas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaFixa = await _context.ContaFixa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contaFixa == null)
            {
                return NotFound();
            }

            return View(contaFixa);
        }

        // POST: ContaFixas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contaFixa = await _context.ContaFixa.FindAsync(id);
            if (contaFixa != null)
            {
                _context.ContaFixa.Remove(contaFixa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContaFixaExists(int id)
        {
            return _context.ContaFixa.Any(e => e.Id == id);
        }        
    }
}
