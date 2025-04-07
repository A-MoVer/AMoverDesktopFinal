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

        [Required(ErrorMessage = "É necessário associar um NIF.")]
        public string NIF { get; set; }

        [Required(ErrorMessage = "É necessário associar um email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "É necessário associar um numero de telefone.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "É necessário associar uma morada.")]
        public string Morada { get; set; }

        [Required(ErrorMessage = "É necessário associar um codigo postal.")]
        public string CodigoPostal { get; set; }

        [Required(ErrorMessage = "É necessário associar uma cidade.")]
        public string Cidade { get; set; }

        // Estado do Cliente
        [Required]
        public EstadoCliente Estado { get; set; } = EstadoCliente.Ativo;

        // Histórico
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataModificacao { get; set; }
        public DateTime? UltimaEncomenda { get; set; }

        public ICollection<OrdemProducao>? OrdensProducao { get; set; }

    }
}
