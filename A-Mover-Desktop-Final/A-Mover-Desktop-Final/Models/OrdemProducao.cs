using System.ComponentModel.DataAnnotations;

namespace A_Mover_Desktop_Final.Models
{
    public enum EstadoOrdemProducao
    {
        Pendente,
        EmProducao,
        Concluida
    }
    public class OrdemProducao
    {
        [Key]
        public int IDOrdemProducao { get; set; }

        [Required(ErrorMessage = "É necessário associar uma encomenda.")]
        public int IDEncomenda { get; set; }
        public Encomenda? Encomenda { get; set; }

        [Required(ErrorMessage = "Número da Ordem de Produção obrigatório")]
        public string NumeroOrdem { get; set; }
        public EstadoOrdemProducao Estado { get; set; }
        public string PaisDestino { get; set; }
        public DateTime DataCriacao { get; set; }

        public DateTime? DataConclusao { get; set; }

        public ICollection<ChecklistMontagem>? ChecklistMontagem { get; set; }
        public ICollection<ChecklistControlo>? ChecklistControlo { get; set; }
        public ICollection<ChecklistEmbalagem>? ChecklistEmbalagem { get; set; }

    }
}
