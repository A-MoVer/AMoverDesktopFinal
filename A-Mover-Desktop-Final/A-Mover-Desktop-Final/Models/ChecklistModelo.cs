using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class ChecklistModelo
    {
        [Key]
        public int ID { get; set; }
        
        public int IDChecklist { get; set; }
        [ForeignKey("IDChecklist")]
        public Checklist? Checklist { get; set; }
        
        public int IDModelo { get; set; }
        [ForeignKey("IDModelo")]
        public ModeloMota? ModeloMota { get; set; }
    }
}