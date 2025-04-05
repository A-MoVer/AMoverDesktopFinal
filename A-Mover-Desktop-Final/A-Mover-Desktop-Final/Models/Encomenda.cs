using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
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

        public DateTime DateCriacao { get; set; }
        public DateTime? DataEntrega { get; set; }
    }
}
