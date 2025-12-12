using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace A_Mover_Desktop_Final.Models
{
    public enum TipoChecklist
    {
        Controlo,
        Embalagem,
        Montagem
    }
    public class Checklist
    {
        [Key]
        public int IDChecklist { get; set; }

        [Required(ErrorMessage = "É necessário associar um nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É necessário associar uma descrição.")]
        public string Descricao { get; set; }

        [Required]
        public TipoChecklist Tipo { get; set; }
        
        // Collection of models associated with this checklist
        public ICollection<ChecklistModelo>? ChecklistModelos { get; set; } = new List<ChecklistModelo>();

        // Property to check if the checklist is generic (not associated with any model)
        [NotMapped]
        public bool IsGeneric => ChecklistModelos == null || !ChecklistModelos.Any();
    }
}
