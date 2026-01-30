using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public enum EstadoServico
    {
        Agendado,
        EmCurso,
        Concluido
    }

    public enum TipoServico
    {
        Revisao,
        Manutenção
    }

    public class Servico
    {
        [Key]
        public int IDServico { get; set; }

        [Required]
        public int IDMota { get; set; }

        [ForeignKey("IDMota")]
        public Mota? Mota { get; set; }

        [Required]
        public TipoServico Tipo { get; set; }

        public string? Descricao { get; set; }

        public EstadoServico Estado { get; set; } = EstadoServico.Agendado;

        [DataType(DataType.Date)]
        public DateTime DataServico { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? DataConclusao { get; set; }

        public string? NotasServico { get; set; }

        public ICollection<ServicosPecasAlteradas>? PecasAlteradas { get; set; }

        [Column("MecanicoId")]
        public int? IDMecanico { get; set; }

        [ForeignKey(nameof(IDMecanico))]
        public Mecanico? Mecanico { get; set; }


    }
}