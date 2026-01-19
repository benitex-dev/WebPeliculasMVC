using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sistema_Web_Peliculas_MVC.Models;
using Sistema_Web_Peliculas_MVC.Services;

namespace Sistema_Web_Peliculas_MVC.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ImagenStorage _imagenStorage;
        private readonly IEmailService _emailService;
        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ImagenStorage imagenStorage, IEmailService emailService)
        {
                _userManager = userManager;
                _signInManager = signInManager;
                _imagenStorage = imagenStorage;
                _emailService = emailService;
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
                    ImagenUrlPerfil = "/images/default-avatar.jpg"
                };  
                var resultado =await _userManager.CreateAsync(nuevoUsuario, usuario.Password);
                if (resultado.Succeeded)
                {
                    await _signInManager.SignInAsync(nuevoUsuario, isPersistent: false);
                    await _emailService.SendAsync(nuevoUsuario.Email, "Bienvenido a Sistema Web Películas", "<h1>Gracias por registrarte en nuestro sitio web de películas.</h1><p>Esperamos que disfrutes la plataforma</p>");
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

        [Authorize]
        public async Task<IActionResult> MiPerfil()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
           
            
            var usuario = new MiPerfilViewModel
            {
                Nombre = usuarioActual.Nombre,
                Apellido = usuarioActual.Apellido,
                Email = usuarioActual.Email,
                ImagenUrlPerfil = usuarioActual.ImagenUrlPerfil
            };
            return View(usuario);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MiPerfil(MiPerfilViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var usuarioActual = await _userManager.GetUserAsync(User);

                try
                {
                    // Aquí podrías agregar lógica para manejar la carga de la nueva imagen de perfil si es necesario
                    if(modelo.ImagenPerfil is not null && modelo.ImagenPerfil.Length > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(usuarioActual.ImagenUrlPerfil))
                        {
                            await _imagenStorage.DeleteAsync(usuarioActual.ImagenUrlPerfil);
                        }
                        var nuevaRuta = await _imagenStorage.SaveAsync(usuarioActual.Id, modelo.ImagenPerfil);

                        usuarioActual.ImagenUrlPerfil = nuevaRuta;
                        modelo.ImagenUrlPerfil = nuevaRuta;
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty,  ex.Message);
                    return View(modelo);
                }

                usuarioActual.Nombre = modelo.Nombre;
                usuarioActual.Apellido = modelo.Apellido;
                
                // Aquí podrías agregar lógica para actualizar la imagen de perfil si es necesario
                var resultado = await _userManager.UpdateAsync(usuarioActual);
                
                if (resultado.Succeeded)
                {
                    ViewBag.Message = "Perfil actualizado exitosamente.";
                    return View(modelo);
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(modelo);
        }
    }
}
