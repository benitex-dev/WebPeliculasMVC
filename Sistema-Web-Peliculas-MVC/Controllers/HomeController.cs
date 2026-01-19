using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Web_Peliculas_MVC.Data;
using Sistema_Web_Peliculas_MVC.Models;
using Sistema_Web_Peliculas_MVC.Services;

namespace Sistema_Web_Peliculas_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LlmService _llmService;
        private readonly MovieDbContext _context;

        public HomeController(ILogger<HomeController> logger,MovieDbContext context,LlmService llmService)
        {
            _logger = logger;
            _context = context;
            _llmService = llmService;
        }

        public async Task<IActionResult> Index(int page = 1, string txtBusqueda="",int generoId=0)
        {
            const int pageSize = 5;
            if (page < 1) page = 1;

            var consulta = _context.Peliculas.AsQueryable();
            
            if (!string.IsNullOrEmpty(txtBusqueda))
            {
                consulta = consulta.Where(p => p.Titulo.Contains(txtBusqueda));
            }
            
            if (generoId > 0)
            {
                consulta = consulta.Where(p => p.GeneroId == generoId);
            }

            var totalItems = await consulta.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);

            if (totalPages == 0) totalPages = 1;
            if (page > totalPages) page = totalPages;

            var peliculas = await consulta
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TxtBusqueda = txtBusqueda;
           
            var generos = await _context.Generos.OrderBy(g => g.Descripcion).ToListAsync();
            generos.Insert(0, new Genero { Id = 0, Descripcion = "Todos" });
            ViewBag.GeneroId = new SelectList(generos,
                "Id", 
                "Descripcion",
                generoId);

            return View(peliculas);
        }
        public async  Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();
            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .Include(p => p. ListaReviews)
                .ThenInclude(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (pelicula == null) return NotFound();

            ViewBag.UserReview = false;
            if(User?.Identity?.IsAuthenticated ==true && pelicula.ListaReviews != null)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.UserReview = !(pelicula.ListaReviews.FirstOrDefault(r => r.UsuarioId == userId) == null);
            }



           
            return View(pelicula);
        }
        [HttpGet]
        public async Task<IActionResult> Spoiler(string titulo)
        {
            try
            {
                var spoiler = await _llmService.ObtenerSpoilerAsync(titulo);
                return Json(new {success=true,data=spoiler});
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Resumen(string titulo)
        {
            try
            {
                var resumen = await _llmService.ObtenerResumenAsync(titulo);
                return Json(new { success = true, data = resumen });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
