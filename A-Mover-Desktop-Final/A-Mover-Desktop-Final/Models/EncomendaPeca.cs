using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public enum EstadoEncomendaPeca
    {
        Pendente,
        Enviada,
        Confirmada,
        Recusada,
        Entregue
    }

    public class EncomendaPeca
    {
        [Key]
        public int IDEncomendaPeca { get; set; }

        [Required]
        [Display(Name = "Peça")]
        public int IDPeca { get; set; }

        [ForeignKey(nameof(IDPeca))]
        public Pecas? Peca { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que 0.")]
        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Necessária")]
        public DateTime DataNecessaria { get; set; } = DateTime.Today;

        [StringLength(500)]
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [Display(Name = "Estado")]
        public EstadoEncomendaPeca Estado { get; set; } = EstadoEncomendaPeca.Pendente;

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
