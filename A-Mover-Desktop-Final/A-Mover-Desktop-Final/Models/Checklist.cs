using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        // Relacionamento com ModeloMota (null significa que é genérico)
        public int? IDModelo { get; set; }
        
        [ForeignKey("IDModelo")]
        public ModeloMota? ModeloMota { get; set; }
    }
}
