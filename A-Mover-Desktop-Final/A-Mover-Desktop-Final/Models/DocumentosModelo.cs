using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public class DocumentosModelo
    {
        [Key]
        public int IDDocumentosModelo { get; set; }

        [Required(ErrorMessage = "É necessário associar um modelo.")]
        public int IDModelo { get; set; }
        [ForeignKey("IDModelo")]
        public ModeloMota? ModeloMota { get; set; }

        [Required(ErrorMessage = "É necessário associar um tipo de documento.")]
        public int IDDocumento { get; set; }
        [ForeignKey("IDModelo")]
        public Documento? Documento { get; set; }

        public string Caminho { get; set; }

    }
}
