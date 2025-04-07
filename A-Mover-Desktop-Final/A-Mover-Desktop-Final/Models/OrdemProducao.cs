using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("IDEncomenda")]
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
        public Mota? Mota { get; set; }

        // Propriedades de navegação para Cliente e Modelo via Encomenda
        [NotMapped]
        public Cliente? Cliente => Encomenda?.Cliente;

        [NotMapped]
        public ModeloMota? Modelo => Encomenda?.ModeloMota;

    }
}
