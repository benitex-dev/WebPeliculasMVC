using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sistema_Web_Peliculas_MVC.Models;

namespace Sistema_Web_Peliculas_MVC.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
                _userManager = userManager;
                _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                // Lógica de autenticación aquí
               var resultado = await _signInManager
                               .PasswordSignInAsync(usuario.Email, usuario.Password,usuario.RememberMe, lockoutOnFailure: false);
                if (resultado.Succeeded)
                {
                     return RedirectToAction("Index", "Home");
                }
                else
                {
                     ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
                }

            }
            return View(usuario);
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                // Lógica de registro aquí
                var nuevoUsuario = new Usuario
                {
                    UserName = usuario.Email,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    ImagenUrlPerfil = "default-profile.png"
                };  
                var resultado =await _userManager.CreateAsync(nuevoUsuario, usuario.Password);
                if (resultado.Succeeded)
                {
                    await _signInManager.SignInAsync(nuevoUsuario, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(usuario);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }   
    }
}
