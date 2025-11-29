using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Models;

namespace PerFinanc.Web.Data
{
    public class PerFinancDbContext : DbContext
    {
        public PerFinancDbContext(DbContextOptions<PerFinancDbContext> options)
            : base(options)
        {
        }
        public DbSet<PerFinanc.Web.Models.ContaFixa> ContaFixa { get; set; }
        public DbSet<PerFinanc.Web.Models.LancamentoContaFixa> LancamentoContaFixa { get; set; }
        public DbSet<PerFinanc.Web.Models.ReceitaEntrada> ReceitaEntrada { get; set; } = default!;
        public DbSet<PerFinanc.Web.Models.Freelance> Freelance { get; set; } = default!;
        public DbSet<PerFinanc.Web.Models.GastoGeral> GastoGeral { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LancamentoContaFixa>()
                .HasIndex(l => new { l.Ano, l.Mes, l.ContaFixaId });
        }   
       
    }
}
