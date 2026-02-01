using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Data;
using PerFinanc.Web.Models;
using System.Security.Claims;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["ContaFixaId"] = new SelectList(_context.ContaFixa.Where(c => c.UserId == userId), "Id", "Nome");
            return View();
        }
        
        // POST: LancamentosContasFixas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContaFixaId,Ano,Mes,ValorPago,DataPagamento,Observacao")] LancamentoContaFixa lancamentoContaFixa)
        {
            // Campos calculados no servidor (se forem [Required], isso evita ModelState inválido)
            ModelState.Remove(nameof(LancamentoContaFixa.ValorPrevisto));
            ModelState.Remove(nameof(LancamentoContaFixa.DataVencimento));
            ModelState.Remove(nameof(ContaFixa.UserId));
            
            var erros = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { Campo = x.Key, Erros = x.Value.Errors.Select(e => e.ErrorMessage).ToList() })
                .ToList();

            // User logado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Se já tiver erro de validação, volta com combo filtrado
            if (!ModelState.IsValid)
            {
                ViewData["ContaFixaId"] = new SelectList(_context.ContaFixa.Where(c => c.UserId == userId), "Id", "Nome", lancamentoContaFixa.ContaFixaId);
                TempData["MensagemErro"] = "Erro na gravação do registro";
                return View(lancamentoContaFixa);
            }
            
            // 1) Busca a ContaFixa pra pegar DiaVencimento e ValorPadrao
            var conta = await _context.ContaFixa.FirstOrDefaultAsync(c => c.Id == lancamentoContaFixa.ContaFixaId && c.UserId == userId);
            if (conta == null)
            {
                ModelState.AddModelError(nameof(lancamentoContaFixa.ContaFixaId), "Conta fixa não encontrada.");
                TempData["MensagemErro"] = "Erro na gravação do registro";
                ViewData["ContaFixaId"] = new SelectList(_context.ContaFixa.Where(c => c.UserId == userId), "Id", "Nome", lancamentoContaFixa.ContaFixaId);
                return View(lancamentoContaFixa);
            }

            // 2) Impede duplicidade (mesma conta fixa no mesmo mês/ano)
            var existe = await _context.LancamentoContaFixa.AnyAsync(l => l.ContaFixaId == lancamentoContaFixa.ContaFixaId
                                                                                        && l.Ano == lancamentoContaFixa.Ano
                                                                                        && l.Mes == lancamentoContaFixa.Mes);

            if (existe)
            {
                ModelState.AddModelError("", "Já existe lançamento desta conta fixa para este mês/ano");
                ViewData["ContaFixaId"] = new SelectList(_context.ContaFixa.Where(c => c.UserId == userId),
                                                            "Id", "Nome", lancamentoContaFixa.ContaFixaId);

                return View(lancamentoContaFixa);
            }

            // 3 Define ValorPrevisto baseado na ContaFixa
            lancamentoContaFixa.ValorPrevisto = conta.ValorPadrao;

            // 4 Calcula DataVencimento baseado no Ano/Mes e no DiaVencimento da ContaFixa
            lancamentoContaFixa.DataVencimento = CalcularVencimento(lancamentoContaFixa.Ano, lancamentoContaFixa.Mes, conta.DiaVencimento);

            // 4) Salva
            _context.Add(lancamentoContaFixa);            
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Registro criado com sucesso!";            
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

        // GET: /LancamentoContaFixa/Pagar/5
        public async Task<IActionResult> Pagar(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var lanc = await _context.LancamentoContaFixa
                .Include(x => x.ContaFixa)
                .FirstOrDefaultAsync(x => x.Id == id && x.ContaFixa!.UserId == userId);

            if (lanc == null) return NotFound();

            // sugestões padrão
            if (!lanc.DataPagamento.HasValue)
                lanc.DataPagamento = DateTime.Today;

            if (!lanc.ValorPago.HasValue || lanc.ValorPago <= 0)
                lanc.ValorPago = lanc.ValorPrevisto;

            return View(lanc);
        }

        // POST: /LancamentoContaFixa/Pagar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pagar(int id, decimal? valorPago, DateTime? dataPagamento, string? observacao)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var lanc = await _context.LancamentoContaFixa
                .Include(x => x.ContaFixa)
                .FirstOrDefaultAsync(x => x.Id == id && x.ContaFixa!.UserId == userId);

            if (lanc == null) return NotFound();

            // aplica mudanças
            lanc.ValorPago = valorPago;
            lanc.DataPagamento = dataPagamento;
            lanc.Observacao = observacao;

            // valida o IValidatableObject
            TryValidateModel(lanc);

            if (!ModelState.IsValid)
                return View(lanc);

            await _context.SaveChangesAsync();

            // volta pro dashboard do mesmo mês/ano
            return RedirectToAction("Index", "Dashboard", new { ano = lanc.Ano, mes = lanc.Mes });
        }

        // POST: /LancamentoContaFixa/Estornar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Estornar(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var lanc = await _context.LancamentoContaFixa
                .Include(x => x.ContaFixa)
                .FirstOrDefaultAsync(x => x.Id == id && x.ContaFixa!.UserId == userId);

            if (lanc == null) return NotFound();

            lanc.ValorPago = null;
            lanc.DataPagamento = null;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Dashboard", new { ano = lanc.Ano, mes = lanc.Mes });
        }
    }
}
