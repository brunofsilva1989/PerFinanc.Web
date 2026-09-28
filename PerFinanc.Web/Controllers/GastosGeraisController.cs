using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Data;
using PerFinanc.Web.Enums;
using PerFinanc.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PerFinanc.Web.Controllers
{
    [Authorize]
    public class GastosGeraisController : Controller
    {
        private readonly PerFinancDbContext _context;
        private readonly IStepLogger _log;

        public GastosGeraisController(PerFinancDbContext context, IStepLogger logger)
        {
            _log = logger;
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
        public async Task<IActionResult> Create([Bind("Id,Descricao,Valor,DataGasto,Categoria,Ano,Mes")] GastoGeral gastoGeral)
        {
            _log.Info("Tentando criar novo registro de gasto geral: " + gastoGeral.Descricao);

            // Campos calculados no servidor (se forem [Required], isso evita ModelState inválido)
            ModelState.Remove(nameof(LancamentoContaFixa.ValorPrevisto));
            ModelState.Remove(nameof(LancamentoContaFixa.DataVencimento));
            ModelState.Remove(nameof(ContaFixa.UserId));

            _log.Info("Validando dados do formulário...");

            var erros = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { Campo = x.Key, Erros = x.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                .ToList();

            _log.Info("Erros encontrados: " + erros.Count);

            // User logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _log.Info("Associando gasto geral ao usuário: " + userId);

            if (ModelState.IsValid)
            {
                _log.Info("Dados válidos. Criando registro de gasto geral...");
                _context.Add(gastoGeral);
                await _context.SaveChangesAsync();
                _log.Info("Registro criado com ID: " + gastoGeral.Id);
                TempData["Mensagem"] = "Registro criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _log.Error("Erro ao criar registro: " + ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage);
                TempData["Mensagem"] = "Erro ao criar registro. Verifique os dados e tente novamente.";
                return View(gastoGeral);
            }
            
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
            // Campos calculados no servidor (se forem [Required], isso evita ModelState inválido)
            ModelState.Remove(nameof(LancamentoContaFixa.ValorPrevisto));
            ModelState.Remove(nameof(LancamentoContaFixa.DataVencimento));
            ModelState.Remove(nameof(ContaFixa.UserId));

            _log.Info("Validando dados do formulário...");

            var erros = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { Campo = x.Key, Erros = x.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                .ToList();

            _log.Info("Erros encontrados: " + erros.Count);

            // User logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _log.Info("Associando gasto geral ao usuário: " + userId);

            if (id != gastoGeral.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _log.Info("Atualizando registro de gasto geral com ID: " + gastoGeral.Id);

                    _context.Update(gastoGeral);
                    TempData["Mensagem"] = "Registro atualizado com sucesso!";
                    await _context.SaveChangesAsync();
                    _log.Info("Registro atualizado com sucesso com ID: " + gastoGeral.Id);
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
            _log.Info("Tentando acessar página de exclusão para gasto geral com ID: " + id);

            if (id == null)
            {
                return NotFound();
            }

            _log.Info("Buscando gasto geral com ID: " + id);

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
