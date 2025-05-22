using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public enum EstadoEncomenda
    {
        Pendente,
        EmProducao,
        Concluida,
        Entregue
    }
    public class Encomenda
    {
        [Key]
        public int IDEncomenda { get; set; }

        [Required(ErrorMessage = "É necessário associar um modelo.")]
        public int IDModelo { get; set; }
        [ForeignKey("IDModelo")]
        public ModeloMota? ModeloMota { get; set; }

        [Required(ErrorMessage = "É necessário associar um Cliente.")]
        public int IDCliente { get; set; }
        [ForeignKey("IDCliente")]
        public Cliente? Cliente { get; set; }

        [Required(ErrorMessage = "É necessário adicionar a quantidade de motas.")]
        public int Quantidade { get; set; }
        public EstadoEncomenda Estado { get; set; } = EstadoEncomenda.Pendente;
        public DateTime DateCriacao { get; set; } = DateTime.Now;
        public DateTime? DataEntrega { get; set; }

        [InverseProperty("Encomenda")]
    public ICollection<OrdemProducao>? OrdemProducao { get; set; }

    // Construtor para inicializar a coleção
    public Encomenda()
    {
        OrdemProducao = new List<OrdemProducao>();
    }
    }
}
