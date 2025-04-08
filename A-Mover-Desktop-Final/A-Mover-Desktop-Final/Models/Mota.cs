using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public enum EstadoMota
    {
        EmProdução,
        Ativo,
        EmManutenção,
        Descontinuado
    }
    public class Mota
    {
        [Key]
        public int IDMota { get; set; }

        [Required(ErrorMessage = "É necessário associar um modelo.")]
        public int IDModelo { get; set; }

        [ForeignKey("IDModelo")]
        public ModeloMota? ModeloMota { get; set; }

        [Required(ErrorMessage = "É necessário associar uma OP.")]
        public int IDOrdemProducao { get; set; }

        [ForeignKey("IDOrdemProducao")]
        public OrdemProducao? OrdemProducao { get; set; }

        [Required(ErrorMessage = "Numero de Identificação Obrigatório")]
        public string NumeroIdentificacao { get; set; }
        public DateTime DataRegisto { get; set; }

        [Required]
        public string Cor { get; set; }

        [Required]
        public double Quilometragem { get; set; }
        public EstadoMota Estado { get; set; }

        public ICollection<MotasPecasSN>? MotasPecasSN { get; set; } = new List<MotasPecasSN>();

    }
}
