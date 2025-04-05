using System.ComponentModel.DataAnnotations;

namespace A_Mover_Desktop_Final.Models
{
    public class Documento
    {
        [Key]
        public int IDDocumento { get; set; }

        [Required(ErrorMessage = "O tipo de documento é obrigatório preencher ")]
        public string Nome { get; set; }
    }
}
