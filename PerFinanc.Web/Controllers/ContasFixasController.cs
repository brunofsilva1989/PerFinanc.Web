using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PerFinanc.Web.Auth;
using PerFinanc.Web.Data;
using PerFinanc.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PerFinanc.Web.Controllers
{
    [Authorize]
    public class ContasFixasController : Controller
    {
        private readonly PerFinancDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContasFixasController(PerFinancDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ContaFixas
        public async Task<IActionResult> Index()
        {
            var userId = UserIdAtual();
            return View(await _context.ContaFixa.Where(c => c.UserId == userId).ToListAsync());
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
        public async Task<IActionResult> Create(ContaFixa contaFixa)
        {
            ModelState.Remove(nameof(ContaFixa.UserId));

            if (ModelState.IsValid)
            {
                contaFixa.UserId =
                User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;

                _context.Add(contaFixa);
                await _context.SaveChangesAsync();
                TempData["Mensagem"] = "Registro criado com sucesso!";
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
        public async Task<IActionResult> Edit(int id, ContaFixa contaFixa)
        {
            if (id != contaFixa.Id)
                return NotFound();
            
            ModelState.Remove(nameof(ContaFixa.UserId));

            if (!ModelState.IsValid)
                return View(contaFixa);

            // SETA NO SERVIDOR
            contaFixa.UserId =
                User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;

            _context.Update(contaFixa);
            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "Registro atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: ContaFixas/Delete/5
        [HttpGet]
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
            TempData["Mensagem"] = "Registro excluido com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool ContaFixaExists(int id)
        {
            return _context.ContaFixa.Any(e => e.Id == id);
        }

        private string UserIdAtual() => _userManager.GetUserId(User)!;
       
    }
}
