using System.ComponentModel.DataAnnotations;

namespace A_Mover_Desktop_Final.Models
{
    public class OrdemProducao
    {
        [Key]
        public int IDOrdemProducao { get; set; }

        [Required(ErrorMessage = "É necessário associar uma encomenda.")]
        public int IDEncomenda { get; set; }
        public Encomenda? Encomenda { get; set; }

        [Required(ErrorMessage = "É necessário associar um Cliente.")]
        public int IDCliente { get; set; }
        public Cliente? Cliente { get; set; }

        [Required(ErrorMessage = "Número da Ordem de Produção obrigatório")]
        public string NumeroOrdem { get; set; }

        public string Estado { get; set; }

        public string PaisDestino { get; set; }

        public string Descricao { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataConclusao { get; set; }
    }
}
