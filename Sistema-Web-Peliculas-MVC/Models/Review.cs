using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_Peliculas_MVC.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula? Pelicula { get; set; }
        [Range(1,5)]
        public int Rating { get; set; }
        [Required]
        [StringLength(500)]
        public string Comentario { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaReview { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }  
    }
}
