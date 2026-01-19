# WebPeliculasMVC

Sistema Web Películas MVC

Descripción
---------
Aplicación ASP.NET Core (.NET 9) para gestionar y mostrar películas: listados, detalle, búsquedas, reseñas y llamadas a un servicio LLM para generar resúmenes y spoilers. Proyecto pensado para ejecutarse desde Visual Studio 2022 o CLI `dotnet`.

Tecnologías
---------
- .NET 9 / ASP.NET Core MVC (Razor Views)
- Entity Framework Core (SQL Server)
- ASP.NET Identity
- Servicios propios: `LlmService`, `ImagenStorage`

Requisitos
---------
- .NET 9 SDK
- Visual Studio 2022 o VS Code
- SQL Server accesible (local o remoto)

Instalación y ejecución
---------
1. Clona el repositorio:
   - `git clone <repo-url>`
2. Restaurar y compilar:
   - `dotnet restore`
   - `dotnet build`
3. Configura la cadena de conexión en `appsettings.json` (ejemplo):
 { "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=MoviesDb;Trusted_Connection=True;" } }
4. Ejecuta la aplicación:
   - Desde Visual Studio: Ejecutar el proyecto `Sistema-Web-Peliculas-MVC`.
   - Desde CLI: `dotnet run --project Sistema-Web-Peliculas-MVC`

Base de datos y seed
---------
- Al iniciar la app se invoca `DbSeeder.Seed(context)` en `Program.cs` para poblar datos iniciales.
- Si prefieres migraciones:
  - `dotnet ef migrations add Inicial -p Sistema-Web-Peliculas-MVC`
  - `dotnet ef database update -p Sistema-Web-Peliculas-MVC`

Configuración de servicios externos
---------
- `LlmService`: configurar claves/URL según su implementación vía `appsettings.json` o variables de entorno.
- `ImagenStorage`: revisar configuración si se suben imágenes.

Rutas / Endpoints importantes
---------
- `/` — Lista paginada de películas (`Home/Index`).  
- `/Home/Details/{id}` — Detalle de una película.  
- `/Home/Resumen?titulo=...` — JSON con resumen (invoca `LlmService`).  
- `/Home/Spoiler?titulo=...` — JSON con spoiler.

Buenas prácticas antes de producción
---------
- Usar `appsettings.{Environment}.json` y secretos para claves sensibles.
- Habilitar HTTPS/HSTS en producción.
- Revisar permisos y roles de Identity.
- Añadir logging estructurado (Serilog, Application Insights).

Resolución de problemas comunes
---------
- NullReferenceException en arranque: ejecutar con "Break on throw" en el depurador; aislar middleware (p. ej. `MapStaticAssets` / `WithStaticAssets`) comentándolos temporalmente.  
- Problemas con la DB: comprobar cadena de conexión y permisos.
- Fallos en `LlmService`: verificar claves y límites de la API.

Contribuir
---------
- Abrir issues y PRs. Incluye descripción, pasos para reproducir y pruebas si aplica.
- Mantén estilo y convenciones del repositorio; crea `CONTRIBUTING.md` si se añadirán más colaboradores.

Licencia
---------
- Añade `LICENSE` adecuado a tu proyecto (MIT/Proprietary/otro).

Contacto
---------
- Para dudas, abrir un issue en el repositorio o contactar al mantenedor indicado en el `package`/metadatos.

