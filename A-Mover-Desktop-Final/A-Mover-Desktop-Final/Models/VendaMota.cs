using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class VendaMota
    {
        [Key]
        public int IDVendaMota { get; set; }

        [Required(ErrorMessage = "É necessário associar um modelo.")]
        public int IDModelo { get; set; }

        [ForeignKey(nameof(IDModelo))]
        public ModeloMota? ModeloMota { get; set; }

        public int? IDCliente { get; set; }

        [ForeignKey(nameof(IDCliente))]
        public Cliente? Cliente { get; set; }

        [Required(ErrorMessage = "Nome do cliente é obrigatório.")]
        [StringLength(150)]
        public string ClienteNome { get; set; } = string.Empty;

        [StringLength(150)]
        public string? ClienteEmail { get; set; }

        [StringLength(30)]
        public string? ClienteTelefone { get; set; }

        [StringLength(50)]
        public string? ClienteNif { get; set; }

        [Required(ErrorMessage = "Número de série é obrigatório.")]
        [StringLength(100)]
        public string NumeroSerie { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cor é obrigatória.")]
        [StringLength(60)]
        public string Cor { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Quilometragem inválida.")]
        public double Quilometragem { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }

        public DateTime DataVenda { get; set; } = DateTime.Now;
    }
}
