using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_Peliculas_MVC.Models
{
    public class RegistroViewModel
    {

        [Required (ErrorMessage ="Debes ingresar un nombre.")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debes ingresar un apellido.")]
        [StringLength(50)]
        public string Apellido { get; set; }
        [EmailAddress(ErrorMessage = "Ingresa un mail valido.")]
        [Required(ErrorMessage = "El mail es obligatorio.")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
