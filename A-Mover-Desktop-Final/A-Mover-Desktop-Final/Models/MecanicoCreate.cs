using System.ComponentModel.DataAnnotations;

public class MecanicoCreate
{
    [Required, StringLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(30)]
    public string? Telemovel { get; set; }
}
