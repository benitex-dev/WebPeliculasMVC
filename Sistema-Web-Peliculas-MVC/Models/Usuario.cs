using Microsoft.AspNetCore.Identity;

namespace Sistema_Web_Peliculas_MVC.Models
{
    public class Usuario:IdentityUser
    {
       
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string ImagenUrlPerfil { get; set; }
        public List<Favorito>? ListaFavoritos { get; set; }
        public List<Review>? ReviewsUsuarios { get; set; }
    }
}
