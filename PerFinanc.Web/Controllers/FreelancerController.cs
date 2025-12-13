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
    public class FreelancerController : Controller
    {
        private readonly PerFinancDbContext _context;

        public FreelancerController(PerFinancDbContext context)
        {
            _context = context;
        }

        // GET: Freelancer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Freelance.ToListAsync());
        }

        // GET: Freelancer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freelance = await _context.Freelance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (freelance == null)
            {
                return NotFound();
            }

            return View(freelance);
        }

        // GET: Freelancer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Freelancer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Valor,DataRecebimento,Categoria")] Freelance freelance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(freelance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(freelance);
        }

        // GET: Freelancer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freelance = await _context.Freelance.FindAsync(id);
            if (freelance == null)
            {
                return NotFound();
            }
            return View(freelance);
        }

        // POST: Freelancer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Valor,DataRecebimento,Categoria")] Freelance freelance)
        {
            if (id != freelance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(freelance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FreelanceExists(freelance.Id))
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
            return View(freelance);
        }

        // GET: Freelancer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var freelance = await _context.Freelance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (freelance == null)
            {
                return NotFound();
            }

            return View(freelance);
        }

        // POST: Freelancer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var freelance = await _context.Freelance.FindAsync(id);
            if (freelance != null)
            {
                _context.Freelance.Remove(freelance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FreelanceExists(int id)
        {
            return _context.Freelance.Any(e => e.Id == id);
        }
    }
}
