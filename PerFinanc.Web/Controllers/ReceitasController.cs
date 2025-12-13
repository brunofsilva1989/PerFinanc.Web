using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Data;
using PerFinanc.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerFinanc.Web.Controllers
{
    [Authorize]
    public class ReceitasController : Controller
    {
        private readonly PerFinancDbContext _context;

        public ReceitasController(PerFinancDbContext context)
        {
            _context = context;
        }

        // GET: Receitas
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReceitaEntrada.ToListAsync());
        }

        // GET: Receitas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receitaEntrada = await _context.ReceitaEntrada
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receitaEntrada == null)
            {
                return NotFound();
            }

            return View(receitaEntrada);
        }

        // GET: Receitas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Receitas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Valor,DataRecebimento,Categoria")] ReceitaEntrada receitaEntrada)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receitaEntrada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receitaEntrada);
        }

        // GET: Receitas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receitaEntrada = await _context.ReceitaEntrada.FindAsync(id);
            if (receitaEntrada == null)
            {
                return NotFound();
            }
            return View(receitaEntrada);
        }

        // POST: Receitas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Valor,DataRecebimento,Categoria")] ReceitaEntrada receitaEntrada)
        {
            if (id != receitaEntrada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receitaEntrada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceitaEntradaExists(receitaEntrada.Id))
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
            return View(receitaEntrada);
        }

        // GET: Receitas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receitaEntrada = await _context.ReceitaEntrada
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receitaEntrada == null)
            {
                return NotFound();
            }

            return View(receitaEntrada);
        }

        // POST: Receitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receitaEntrada = await _context.ReceitaEntrada.FindAsync(id);
            if (receitaEntrada != null)
            {
                _context.ReceitaEntrada.Remove(receitaEntrada);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceitaEntradaExists(int id)
        {
            return _context.ReceitaEntrada.Any(e => e.Id == id);
        }
    }
}
