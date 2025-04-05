
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "You must agree to the terms.")]
    public bool Terms { get; set; }

    // Propriedade para o dropdown
    [Required(ErrorMessage = "You must select a role.")]
    public string SelectedRole { get; set; }
    public IEnumerable<SelectListItem>? Roles { get; set; }

}
