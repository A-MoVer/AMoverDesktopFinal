using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace A_Mover_Desktop_Final.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<A_Mover_Desktop_Final.Models.Checklist> Checklist { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ChecklistControlo> ChecklistControlo { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ChecklistEmbalagem> ChecklistEmbalagem { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ChecklistMontagem> ChecklistMontagem { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ChecklistModelo> ChecklistModelo { get; set; }

        public DbSet<A_Mover_Desktop_Final.Models.Cliente> Clientes { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.Encomenda> Encomendas { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ModeloMota> ModelosMota { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.OrdemProducao> OrdemProducao { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.Pecas> Pecas { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.Fornecedor> Fornecedores { get; set; } = default!;
        public DbSet<A_Mover_Desktop_Final.Models.Documento> Documento { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.DocumentosModelo> DocumentosModelo { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ModeloPecasFixas> ModeloPecasFixas { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ModeloPecasSN> ModeloPecasSN { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.Mota> Motas { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.MotasPecasSN> MotasPecasSN { get; set; }

        public DbSet<A_Mover_Desktop_Final.Models.MotasPecasInfo> MotasPecasInfo { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.Servico> Servico { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ServicosPecasAlteradas> ServicosPecasAlteradas { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.Utilizador> Utilizadores { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.UtilizadorMota> UtilizadorMota { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.MaterialRecebido> MateriaisRecebidos { get; set; } = default!;

        public DbSet<A_Mover_Desktop_Final.Models.Mecanico> Mecanicos { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // In your OnModelCreating method
            modelBuilder.Entity<MotasPecasSN>()
                .HasOne(m => m.Mota)
                .WithMany(m => m.MotasPecasSN)
                .HasForeignKey(m => m.IDMota);

            modelBuilder.Entity<MotasPecasSN>()
                .HasOne(m => m.Pecas)
                .WithMany()
                .HasForeignKey(m => m.IDPeca);
            modelBuilder.Entity<ModeloPecasSN>()
                .HasOne(m => m.ModeloMota)
                .WithMany()
                .HasForeignKey(m => m.IDModelo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdemProducao>()
                .HasOne(o => o.Encomenda)
                .WithMany()
                .HasForeignKey(o => o.IDEncomenda)
                .OnDelete(DeleteBehavior.Restrict); // ou DeleteBehavior.NoAction

            modelBuilder.Entity<ServicosPecasAlteradas>()
               .HasOne(s => s.Servico)
               .WithMany(s => s.PecasAlteradas)
               .HasForeignKey(s => s.IDServico)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServicosPecasAlteradas>()
                .HasOne(s => s.MotasPecasSN)
                .WithMany()
                .HasForeignKey(s => s.IDMotasPecasSN)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Fornecedor>().ToTable("Fornecedores");
            modelBuilder.Entity<Pecas>().ToTable("Pecas");
            modelBuilder.Entity<MaterialRecebido>().ToTable("MateriaisRecebidos");

            modelBuilder.Entity<Pecas>()
                .HasOne(p => p.Fornecedor)
                .WithMany(f => f.Pecas)
                .HasForeignKey(p => p.FornecedorId)   
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MaterialRecebido>()
                  .HasOne(m => m.Peca)
                  .WithMany()
                  .HasForeignKey(m => m.PecaId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MaterialRecebido>()
                .HasOne(m => m.Fornecedor)
                .WithMany()
                .HasForeignKey(m => m.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mecanico>()
                .HasIndex(m => new { m.OficinaId, m.Email })
                .IsUnique();


        }
    }
}
