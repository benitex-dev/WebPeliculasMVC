using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Web_Peliculas_MVC.Models
{
    public class Pelicula    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(1000)]
        public string Sinopsis { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaLanzamiento { get; set; }
        [Range(1, 500)]
        public int MinutosDuracion { get; set; }
        [Url]
        [Required]
        public string PosterUrlPortada { get; set; }
        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }
        public int PlataformaId { get; set; }
        public Plataforma? Plataforma { get; set; }
        [NotMapped]
        public int PromedioRating { get; set; }
        public List<Review>? ListaReviews { get; set; }
        public List<Favorito>? UsuariosFavoritos { get; set; }
    }
}
