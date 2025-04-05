using System.ComponentModel.DataAnnotations;

namespace A_Mover_Desktop_Final.Models
{
    public class Checklist
    {
        [Key]
        public int IDChecklist { get; set; }

        [Required(ErrorMessage = "É necessário associar um nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É necessário associar uma descrição.")]
        public string Descricao { get; set; }
    }
}
