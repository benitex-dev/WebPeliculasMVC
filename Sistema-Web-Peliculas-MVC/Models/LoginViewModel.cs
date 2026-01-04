using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_Peliculas_MVC.Models
{
    public class LoginViewModel
    {
        [EmailAddress (ErrorMessage ="Ingresa un mail valido.")]
        [Required (ErrorMessage ="El mail es obligatorio.")]
        public string Email { get; set; }

        [PasswordPropertyText]
        [Required (ErrorMessage ="La clave es obligatoria.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
