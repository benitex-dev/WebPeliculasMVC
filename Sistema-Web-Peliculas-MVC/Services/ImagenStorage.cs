using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Sistema_Web_Peliculas_MVC.Services
{
    public class ImagenStorage
    {
        private readonly IWebHostEnvironment _env;
        public ImagenStorage(IWebHostEnvironment env)
        {
            _env = env;
        }

        private static readonly HashSet<string> permittedExtensions = new(StringComparer.OrdinalIgnoreCase) 
        { 
                "image/jpeg",
                "image/png",
                "image/gif",
                "image/webp"
        };

        public async Task<string> SaveAsync(string userId,IFormFile file, CancellationToken ct=default)
        {
            if (file is null || file.Length == 0)
            {
               throw new InvalidOperationException("File is empty");
            }
            if (file.Length > 2 * 1024*1024)
            {
                throw new InvalidOperationException("File size exceeds the 2MB limit");
            }
            if(!permittedExtensions.Contains(file.ContentType))
            {
                throw new InvalidOperationException("File type is not permitted");
            }
            //esto es para eviar el spoofing y no que te metan cualquier archivo, es para validar la firma real
            //puede lanzar excepcion si no es una imagen valida o si el archivo está corrupto
            //hay que manejar esa excepcion aqui y en el controlador
            //en el controlador se puede capturar, guardar en el model state y mostrar un mensaje al usuario
            using var image= await Image.LoadAsync(file.OpenReadStream(), ct);

            //ajustar imagen
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(512, 512),
                Mode = ResizeMode.Crop
            }));

            //elegir extensión de salida(recomiendo webp o jpg)
            var ext = ".webp";
            var folderRel=$"/uploads/avatars/{userId}";
            var folderAbs=Path.Combine(_env.WebRootPath, "uploads", "avatars",userId);
            
            Directory.CreateDirectory(folderAbs);

            var fileName = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Guid.NewGuid():N}{ext}";
            var absPath= Path.Combine(folderAbs, fileName);
            var relPath= $"{folderRel}/{fileName}".Replace("\\","/");

            await image.SaveAsWebpAsync(absPath, ct);//necesita el paquete SixLabors.ImageSharp.Webp
            return relPath;
        }
        public Task DeleteAsync(string? relativePath, CancellationToken ct=default)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return Task.CompletedTask;
            }

                var abs = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
               
            if (File.Exists(abs))
                {
                    File.Delete(abs);
                }
            
            return Task.CompletedTask;
            
        }
    }
}
