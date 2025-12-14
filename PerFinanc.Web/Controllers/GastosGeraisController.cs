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
    public class GastosGeraisController : Controller
    {
        private readonly PerFinancDbContext _context;

        public GastosGeraisController(PerFinancDbContext context)
        {
            _context = context;
        }

        // GET: GastosGerais
        public async Task<IActionResult> Index()
        {
            return View(await _context.GastoGeral.ToListAsync());
        }

        // GET: GastosGerais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gastoGeral = await _context.GastoGeral
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gastoGeral == null)
            {
                return NotFound();
            }

            return View(gastoGeral);
        }

        // GET: GastosGerais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GastosGerais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Valor,DataGasto,Categoria")] GastoGeral gastoGeral)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gastoGeral);
                await _context.SaveChangesAsync();
                TempData["Mensagem"] = "Registro criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(gastoGeral);
        }

        // GET: GastosGerais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gastoGeral = await _context.GastoGeral.FindAsync(id);
            if (gastoGeral == null)
            {
                return NotFound();
            }
            return View(gastoGeral);
        }

        // POST: GastosGerais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Valor,DataGasto,Categoria")] GastoGeral gastoGeral)
        {
            if (id != gastoGeral.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gastoGeral);
                    TempData["Mensagem"] = "Registro atualizado com sucesso!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoGeralExists(gastoGeral.Id))
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
            return View(gastoGeral);
        }

        // GET: GastosGerais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gastoGeral = await _context.GastoGeral
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gastoGeral == null)
            {
                return NotFound();
            }

            return View(gastoGeral);
        }

        // POST: GastosGerais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gastoGeral = await _context.GastoGeral.FindAsync(id);
            if (gastoGeral != null)
            {
                _context.GastoGeral.Remove(gastoGeral);
            }

            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "Registro excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool GastoGeralExists(int id)
        {
            return _context.GastoGeral.Any(e => e.Id == id);
        }
    }
}
