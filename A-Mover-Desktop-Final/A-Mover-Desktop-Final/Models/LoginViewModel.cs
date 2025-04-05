using System.ComponentModel.DataAnnotations;

namespace A_Mover_Desktop_Final.Models
{
    public class LoginViewModel
    {
        [Required]
        public string EmailUsername { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
