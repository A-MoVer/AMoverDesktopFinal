using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class ModeloPecasFixas
    {
        [Key]
        public int IDMPF { get; set; }

        [Required(ErrorMessage = "É necessário associar um modelo.")]
        public int IDModelo { get; set; }

        [ForeignKey("IDModelo")]
        public ModeloMota? ModeloMota { get; set; }

        [Required(ErrorMessage = "É necessário associar uma peça.")]
        public int IDPeca { get; set; }

        [ForeignKey("IDPeca")]
        public Pecas? Pecas { get; set; }

        [StringLength(100)]
        public string? EspecificacaoPadrao { get; set; } // Campo novo para especificação padrão
    }
}
