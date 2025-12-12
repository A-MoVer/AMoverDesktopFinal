using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Mover_Desktop_Final.Models
{
    public enum EstadoModelo
    {
        EmProdução,
        Ativo,
        Descontinuado
    }
    public class ModeloMota
    {
        [Key]
        public int IDModelo { get; set; }

        [Required(ErrorMessage = "O codigo do Modelo é obrigatório preencher")]
        public string CodigoProduto { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome do modelo é obrigatório preencher")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de inicio de Produção é obrigatório preencher")]
        public DateTime DataInicioProducao { get; set; } = DateTime.Now;
        public DateTime? DataLancamento { get; set; }
        public DateTime? DataDescontinuacao { get; set; }
        public EstadoModelo Estado { get; set; }

        // Make these properties public for Entity Framework to map them properly
        public virtual List<ModeloPecasFixas> PecasFixas { get; set; } = new List<ModeloPecasFixas>();
        public virtual List<ModeloPecasSN> PecasSN { get; set; } = new List<ModeloPecasSN>();
        public virtual ICollection<ChecklistModelo>? ChecklistModelos { get; set; } = new List<ChecklistModelo>();

        // Properties for form handling (not stored in database)
        [NotMapped]
        public List<int> SelectedFixedPartIds { get; set; } = new List<int>();

        [NotMapped]
        public List<int> SelectedSNPartIds { get; set; } = new List<int>();

        public ICollection<OrdemProducao>? OrdensProducao { get; set; }

    }
}
