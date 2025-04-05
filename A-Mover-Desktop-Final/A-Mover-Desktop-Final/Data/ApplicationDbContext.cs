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
        public DbSet<A_Mover_Desktop_Final.Models.Cliente> Clientes { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.Encomenda> Encomendas { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ModeloMota> ModelosMota { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.OrdemProducao> OrdemProducao { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.Pecas> Pecas { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.Documento> Documento { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.DocumentosModelo> DocumentosModelo { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ModeloPecasFixas> ModeloPecasFixas { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.ModeloPecasSN> ModeloPecasSN { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.Mota> Motas { get; set; }
        public DbSet<A_Mover_Desktop_Final.Models.MotasPecasSN> PecasModelo { get; set; }
    }
}
