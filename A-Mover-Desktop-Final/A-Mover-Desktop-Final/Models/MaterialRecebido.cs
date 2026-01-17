using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class MaterialRecebido
    {
        [Key]
        public int IDMaterialRecebido { get; set; }

        [Required]
        [Display(Name = "Peça")]
        public int PecaId { get; set; }

        [ForeignKey(nameof(PecaId))]
        public Pecas? Peca { get; set; }

        [Required]
        [Display(Name = "Fornecedor")]
        public int FornecedorId { get; set; }

        [ForeignKey(nameof(FornecedorId))]
        public Fornecedor? Fornecedor { get; set; }

        [Required(ErrorMessage = "A data de receção é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Receção")]
        public DateTime DataRececao { get; set; } = DateTime.Today;

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que 0.")]
        [Display(Name = "Quantidade Recebida")]
        public int Quantidade { get; set; }

        [StringLength(100)]
        public string? Lote { get; set; }

        [StringLength(100)]
        [Display(Name = "Documento")]
        public string? Documento { get; set; }

        [StringLength(300)]
        public string? Observacoes { get; set; }
    }
}
