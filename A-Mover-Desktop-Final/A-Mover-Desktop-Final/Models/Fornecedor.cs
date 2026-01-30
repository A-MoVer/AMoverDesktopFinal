namespace A_Mover_Desktop_Final.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Fornecedor
    {
        [Key]
        public int IDFornecedor { get; set; }

        [Required(ErrorMessage = "O nome do fornecedor é obrigatório.")]
        [StringLength(150)]
        public string Nome { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email inválido.")]
        [StringLength(150)]
        public string? Email { get; set; }

        // 1 Fornecedor -> N Peças
        public ICollection<Pecas> Pecas { get; set; } = new List<Pecas>();
    }
}
