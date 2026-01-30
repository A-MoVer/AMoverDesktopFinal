using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class Compras
    {

        [Key]
        public int IDCompra { get; set; }

        [Required]
        [Display(Name = "Peça")]
        public int PecaId { get; set; }

        [ForeignKey(nameof(PecaId))]
        public Pecas? Peca { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Pedido")]
        public DateTime DataPedido { get; set; } = DateTime.Today;


        [Required]
        [Display(Name = "Fornecedor")]
        public int FornecedorId { get; set; }

        [ForeignKey(nameof(FornecedorId))]
        public Fornecedor? Fornecedor { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que 0.")]
        [Display(Name = "Quantidade Recebida")]
        public int Quantidade { get; set; }



        public ICollection<MaterialRecebido> MateriaisRecebidos { get; set; } = new List<MaterialRecebido>();



    }
}
