using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
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
    }
}
