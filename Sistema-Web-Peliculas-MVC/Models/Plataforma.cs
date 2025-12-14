namespace Sistema_Web_Peliculas_MVC.Models
{
    public class Plataforma
    {
        private int Id { get; set; }
        private string Nombre { get; set; }
        public string Url { get; set; }
        public string LogoUrl { get; set; }
        public List<Pelicula>? PeliculasPlataforma { get; set; }

    }
}
