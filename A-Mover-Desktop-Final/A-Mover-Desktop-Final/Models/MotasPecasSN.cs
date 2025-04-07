using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class MotasPecasSN
    {
        [Key]
        public int IDMotasPecasSN { get; set; }

        [Required(ErrorMessage = "É necessário associar uma mota.")]
        public int IDMota { get; set; }

        [ForeignKey("IDMota")]
        public Mota? Mota { get; set; }

        [Required(ErrorMessage = "É necessário associar uma peça com SN.")]
        public int IDModeloPecaSN { get; set; }

        [ForeignKey("IDModeloPecaSN")]
        public ModeloPecasSN? ModeloPecasSN { get; set; }

        [Required(ErrorMessage = "É necessário associar o numero de série da peça (Serial Number).")]
        public string? NumeroSerie { get; set; }

    }
}
