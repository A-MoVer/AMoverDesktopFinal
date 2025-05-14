using System.ComponentModel.DataAnnotations;

namespace A_Mover_Desktop_Final.Models
{

    public enum EstadoUtilizador
    {
        Ativo,
        Inativo
    }
    public class Utilizador
    {
        [Key]
        public int IdUtilizador { get; set; }
        public string Nome { get; set; }

        public string Email { get; set; }

        public EstadoUtilizador Estado { get; set; } = EstadoUtilizador.Ativo;



    }
}
