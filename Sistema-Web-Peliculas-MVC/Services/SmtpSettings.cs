namespace Sistema_Web_Peliculas_MVC.Services
{
    public sealed class SmtpSettings
    {
        public string Host { get; set; } = "";
        public int Port { get; set; }
        public string User { get; set; } = "";
        public string Password { get; set; } = "";

        public string FromName { get; set; } = "Sistema";
        public bool UseStartTls { get; set; } = true;
    }
}