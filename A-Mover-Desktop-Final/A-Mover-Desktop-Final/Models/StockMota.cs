using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class StockMota
    {
        [Key]
        public int IDStockMota { get; set; }

        [Required(ErrorMessage = "É necessário associar um modelo.")]
        public int IDModelo { get; set; }

        [ForeignKey(nameof(IDModelo))]
        public ModeloMota? ModeloMota { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser maior ou igual a 0.")]
        public int QuantidadeDisponivel { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}
