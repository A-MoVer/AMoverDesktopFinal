using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class ServicosPecasAlteradas
    {
        [Key]
        public int ID { get; set; }

        public int IDServico { get; set; }

        [ForeignKey("IDServico")]
        public Servico? Servico { get; set; }

        public int IDMotasPecasSN { get; set; } // Relacionado com a peça específica da mota

        [ForeignKey("IDMotasPecasSN")]
        public MotasPecasSN? MotasPecasSN { get; set; }

        public string? Observacoes { get; set; }
    }
}