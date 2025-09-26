using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class MotasPecasInfo
    {
        [Key]
        public int IDMotasPecasInfo { get; set; }

        [Required(ErrorMessage = "É necessário associar uma mota.")]
        public int IDMota { get; set; }

        [ForeignKey("IDMota")]
        public Mota? Mota { get; set; }

        [Required(ErrorMessage = "É necessário associar uma peça com SN.")]
        public int IDPeca { get; set; }

        [ForeignKey("IDPeca")]
        public Pecas? Pecas { get; set; }

        [Required(ErrorMessage = "É necessário associar informação adicional da peça.")]
        public string? InformacaoAdicional { get; set; }

    }
}
