using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public enum EstadoAssociacao
    {
        Ativo,
        Inativo,
        Cancelado
    }
    public class UtilizadorMota
    {
        [Key]
        public int IDUtilizadorMota { get; set; }

        [Required]
        public int IDMota { get; set; }

        [ForeignKey("IDMota")]
        public Mota? Mota { get; set; }

        [Required]
        public int IdUtilizador { get; set; }

        [ForeignKey("IdUtilizador")]
        public Utilizador? Utilizador { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Campos adicionados para controle de estado
        public EstadoAssociacao Estado { get; set; } = EstadoAssociacao.Ativo;

        // Novos campos para controle de inativação
        public DateTime? DataInativacao { get; set; }
        public string? MotivoInativacao { get; set; }

    }
}
