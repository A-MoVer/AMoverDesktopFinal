using System.ComponentModel.DataAnnotations;

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
        public string CodigoProduto { get; set; }

        [Required(ErrorMessage = "O nome do modelo é obrigatório preencher")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A data de inicio de Produção é obrigatório preencher")]
        public DateTime DataInicioProducao { get; set; } = DateTime.Now;
        public DateTime? DataLancamento { get; set; }
        public DateTime? DataDescontinuacao { get; set; }
        public EstadoModelo Estado { get; set; }
    }
}
