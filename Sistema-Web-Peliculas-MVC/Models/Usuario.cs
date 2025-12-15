using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_Peliculas_MVC.Models
{
    public class Usuario:IdentityUser
    {

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }

        [DataType(DataType.Date)]   
        public DateTime FechaNacimiento { get; set; }

        public string ImagenUrlPerfil { get; set; }
        public List<Favorito>? ListaFavoritos { get; set; }
        public List<Review>? ReviewsUsuarios { get; set; }
    }
}
