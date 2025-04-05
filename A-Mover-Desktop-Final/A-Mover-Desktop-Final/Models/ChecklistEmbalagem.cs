using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class ChecklistEmbalagem
    {
        [Key]
        public int IDChecklistEmbalagem { get; set; }

        public int IDChecklist { get; set; }
        [ForeignKey("IDChecklist")]
        public Checklist? Checklist { get; set; }

        public int IDOrdemProducao { get; set; }

        [ForeignKey("IDOrdemProducao")]
        public OrdemProducao? OrdemProducao { get; set; }

        public string Estado { get; set; }
    }
}
