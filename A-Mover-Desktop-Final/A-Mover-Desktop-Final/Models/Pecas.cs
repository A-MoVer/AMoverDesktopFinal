using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class Pecas
    {
        [Key]
        public int IDPeca { get; set; }

        [Required(ErrorMessage = "O número da peça (Part Number) é obrigatório preencher")]
        public string PartNumber { get; set; }

        [Required(ErrorMessage = "A descrição da Peça é obrigatório preencher")]
        public string Descricao { get; set; }

        public int FornecedorId { get; set; }

        [ForeignKey(nameof(FornecedorId))]
        public Fornecedor? Fornecedor { get; set; }
    }
}
