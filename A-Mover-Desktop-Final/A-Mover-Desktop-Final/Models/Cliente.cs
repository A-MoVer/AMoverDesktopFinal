using System.ComponentModel.DataAnnotations;

namespace A_Mover_Desktop_Final.Models
{
    public enum TipoCliente
    {
        Particular,
        Empresa
    }

    public enum EstadoCliente
    {
        Ativo,
        Inativo,
        Suspenso
    }
    public class Cliente
    {
        [Key]
        public int IDCliente { get; set; }

        [Required(ErrorMessage = "É necessário associar um nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O tipo de cliente é obrigatório.")]
        public TipoCliente Tipo { get; set; }

        // Histórico
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataModificacao { get; set; }
        public DateTime? UltimaEncomenda { get; set; }

        public ICollection<OrdemProducao>? OrdensProducao { get; set; }

    }
}
