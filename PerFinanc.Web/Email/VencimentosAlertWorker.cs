using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Auth;
using PerFinanc.Web.Data;

namespace PerFinanc.Web.Email
{
    public class VencimentosAlertWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<VencimentosAlertWorker> _logger;

        public VencimentosAlertWorker(IServiceScopeFactory scopeFactory, ILogger<VencimentosAlertWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Vencimentos Alert Worker started at: {time}", DateTimeOffset.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RunOnce(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao executar alertas de vencimentos.");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
            _logger.LogInformation("Vencimentos Alert Worker stopped at: {time}", DateTimeOffset.Now);
        }

        private async Task RunOnce(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PerFinancDbContext>();
            var dbIdentity = scope.ServiceProvider.GetRequiredService<PerFinancIdentityDbContext>();
            var email = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var hoje = DateTime.Today;
            var limite = hoje.AddDays(3);

            var vencendo = await db.LancamentoContaFixa
                .AsNoTracking()
                .Where(l => !l.EstaPago && l.DataVencimento >= hoje && l.DataVencimento <= limite)
                .Include(l => l.ContaFixa)
                .ToListAsync(ct);

            // ✅ Agrupa pelo dono da ContaFixa
            var porUsuario = vencendo
                .Where(x => x.ContaFixa != null)
                .GroupBy(x => x.ContaFixa.UserId);

            foreach (var grupo in porUsuario)
            {
                var userId = grupo.Key;

                var user = await dbIdentity.Users
                    .AsNoTracking()
                    .Where(u => u.Id == userId)
                    .Select(u => new { u.Email })
                    .FirstOrDefaultAsync(ct);

                if (user == null || string.IsNullOrWhiteSpace(user.Email))
                    continue;

                var linhas = string.Join("", grupo
                    .OrderBy(x => x.DataVencimento)
                    .Select(x =>
                        $"<li><b>{x.ContaFixa.Nome}</b> — vence em {x.DataVencimento:dd/MM/yyyy} — R$ {x.ValorPrevisto:n2}</li>"
                    )
                );

                var html = $@"
                            <h2>Alertas de Vencimentos Próximos</h2>
                            <p>Os seguintes compromissos estão com vencimento próximo:</p>
                            <ul>{linhas}</ul>
                            <p>PerFinanc</p>";

                await email.SendEmailAsync(user.Email, "⚠️ Contas próximas do vencimento", html, ct);
            }
        }
    }
}
