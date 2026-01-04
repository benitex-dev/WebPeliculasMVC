using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_Peliculas_MVC.Models
{
    public class RegistroViewModel
    {

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }

        public string Email { get; set; }   

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
