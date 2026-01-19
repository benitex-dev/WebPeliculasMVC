using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sistema_Web_Peliculas_MVC.Data;
using Sistema_Web_Peliculas_MVC.Models;

namespace Sistema_Web_Peliculas_MVC.Controllers
{
    public class ReviewController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly MovieDbContext _context;
        public ReviewController(UserManager<Usuario> userManager,MovieDbContext context)
        {
                _userManager = userManager;
                _context = context;
        }
        // GET: ReviewController
        public async Task<ActionResult> Index()
        {
            var userId = _userManager.GetUserId(User); 
            var reviews = await _context.Reviews
                .Include(r => r.Pelicula)
                .Where(r => r.UsuarioId == userId)
                .ToListAsync();
            return View(reviews);
        }

        // GET: ReviewController/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int id)
        {   

            return View();
        }

        // GET: ReviewController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReviewController/Create

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateReviewViewModel model)
        
        {
            try
            {
                model.UsuarioId = _userManager.GetUserId(User);

                //validacion simple para evitar que un usuario haga mas de una review por pelicula
                    var reviewExistente = _context.Reviews
                        .FirstOrDefault(r => r.PeliculaId == model.PeliculaId && r.UsuarioId == model.UsuarioId);
                if (reviewExistente != null)
                { 
                    TempData["ReviewExiste"] = "Ya has realizado una reseña para esta película.";
                    return RedirectToAction("Details","Home", new {id=model.PeliculaId});
                }
                //fin validacion

                // Guardar la review si el modelo es válido
                if (ModelState.IsValid)
                {
                    var review = new Review
                    {
                        PeliculaId = model.PeliculaId,
                        UsuarioId = model.UsuarioId,
                        Rating = model.Rating,
                        Comentario = model.Comentario,
                        FechaReview = DateTime.Now
                    };
                    _context.Reviews.Add(review);
                    _context.SaveChanges();
                    return RedirectToAction("Details","Home", new {id=review.PeliculaId});

                }
                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        // GET: ReviewController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (review.UsuarioId != user.Id &&  !_userManager.IsInRoleAsync(user,"Admin").Result)
            {
                return Forbid();
            }

            var reviewViewModel = new CreateReviewViewModel
            {
                Id = review.Id,
                PeliculaId = review.PeliculaId,
                UsuarioId = review.UsuarioId,
                Rating = review.Rating,
                Comentario = review.Comentario
            };
            return View(reviewViewModel);
        }

        // POST: ReviewController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateReviewViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var reviewExistente = _context.Reviews
                        .FirstOrDefault(r => r.Id == model.Id);
                    if (reviewExistente == null) 
                    {
                        return NotFound();
                    }


                    var user = await _userManager.GetUserAsync(User);
                    if (model.UsuarioId != user.Id && !_userManager.IsInRoleAsync(user, "Admin").Result)
                    {
                        return Forbid();
                    }


                    reviewExistente.Rating = model.Rating;
                    reviewExistente.Comentario = model.Comentario;
                    _context.Reviews.Update(reviewExistente);
                    _context.SaveChanges();
                    return RedirectToAction("Index","Review");
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        // GET: ReviewController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReviewController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
