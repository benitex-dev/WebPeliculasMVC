using System.ComponentModel.DataAnnotations;

namespace Sistema_Web_Peliculas_MVC.Models
{
    public class CreateReviewViewModel
    {
        public int? Id { get; set; }
        public int PeliculaId { get; set; }
        public string? PeliculaTItulo { get; set; }
        public string UsuarioId { get; set; }= string.Empty;
        [Range(1, 5, ErrorMessage = "El rating debe estar entre 1 y 5")]
        [Required(ErrorMessage = "El rating es obligatorio")]
        public int Rating { get; set; }
        [StringLength(500, ErrorMessage = "El comentario no puede exceder los 500 caracteres")]
        [Required(ErrorMessage = "El comentario es obligatorio")]
        public string Comentario { get; set; }= string.Empty;
    }
}
