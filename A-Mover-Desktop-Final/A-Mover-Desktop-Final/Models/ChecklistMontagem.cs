using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public enum VerificadoChecklistMontagem
    {
        Sim,
        Nao
    }
    public class ChecklistMontagem
    {
        [Key]
        public int IDChecklistMontagem { get; set; }

        public int IDChecklist { get; set; }
        [ForeignKey("IDChecklist")]
        public Checklist? Checklist { get; set; }

        public int IDOrdemProducao { get; set; }

        [ForeignKey("IDOrdemProducao")]
        public OrdemProducao? OrdemProducao { get; set; }

        public VerificadoChecklistMontagem Verificado { get; set; } = VerificadoChecklistMontagem.Nao;

    }
}
