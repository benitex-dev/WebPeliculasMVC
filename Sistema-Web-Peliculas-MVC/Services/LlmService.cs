using OpenAI.Responses;

namespace Sistema_Web_Peliculas_MVC.Services
{
    public class LlmService
    {
        private readonly string _apiKey;
        private readonly string _model;

        public LlmService()
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set.");

            _model = "gpt-4o-mini"; // or "gpt-4o-mini"
        }
        public async Task<string> ObtenerSpoilerAsync(string tituloPelicula)
        {
            if(string.IsNullOrWhiteSpace(tituloPelicula))
            {
                throw new ArgumentException("El título de la película no puede estar vacío.", nameof(tituloPelicula));
            }

            string prompt = $"Proporciona un pequeño spoiler de la película titulada '{tituloPelicula}'.";
            return await ConsultarLlmAsync(prompt);

        }

        
        public async Task<string> ObtenerResumenAsync(string tituloPelicula)
        {
            if(string.IsNullOrWhiteSpace(tituloPelicula))
            {
                throw new ArgumentException("El titulo de la pelicula no puede estar vacío.", nameof(tituloPelicula));
            }
            string prompt = $"Proporciona un pequeño resumen de la película titulada '{tituloPelicula}'.";
            return await ConsultarLlmAsync(prompt);
        }

        private async Task<string> ConsultarLlmAsync(string prompt)
        {
            try
            {
                OpenAIResponseClient client;
                client = new(apiKey: _apiKey,model:_model);

                OpenAIResponse response;

                response = await client.CreateResponseAsync(prompt);

                string assitantResponse = response.GetOutputText();
                return assitantResponse?.Trim() ?? "No se puede obtener respuestas";
            }
            catch (HttpRequestException ex)
            {    
                    throw new InvalidOperationException($"Error al conectar con el servicio LLM: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error inesperado al consultar el servicio LLM: {ex.Message}", ex);
            }
        }

        public async Task<string> ConsultaSimpleAsync(string pregunta)
        {
            if(string .IsNullOrWhiteSpace(pregunta))
            {
                throw new ArgumentException("La pregunta no puede estar vacía.", nameof(pregunta));
            }
            return await ConsultarLlmAsync(pregunta);
        }


    }
}
