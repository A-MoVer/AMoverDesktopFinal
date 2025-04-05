using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class Mota
    {
        [Key]
        public int IDMota { get; set; }

        [Required(ErrorMessage = "É necessário associar um modelo.")]
        public int IDModelo { get; set; }

        [ForeignKey("IDModelo")]
        public ModeloMota? ModeloMota { get; set; }

        public DateTime DataRegisto { get; set; }

        [Required]
        public string Cor { get; set; }

        [Required]
        public double Quilometragem { get; set; }
        public string Estado { get; set; }
    }
}
