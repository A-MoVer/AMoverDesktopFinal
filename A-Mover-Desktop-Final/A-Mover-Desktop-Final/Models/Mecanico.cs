using System.ComponentModel.DataAnnotations;

namespace A_Mover_Desktop_Final.Models
{
    public class Mecanico
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(160)]
        public string Email { get; set; } = string.Empty;

        [Phone, StringLength(30)]
        public string? Telemovel { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Oficina a que pertence
        public String OficinaId { get; set; }

        public string? UserId { get; set; }

        public bool MustChangePassword { get; set; } = true;

        public ICollection<Servico>? Servicos { get; set; }


    }
}
