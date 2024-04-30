namespace PanoramaApi
{
    public static class AppSettings
    {
        static IConfigurationRoot _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        static internal readonly string JwtIssuer = _configuration["Jwt:Issuer"] ?? string.Empty;
        static internal readonly string JwtKey = _configuration["Jwt:Key"] ?? string.Empty;
        static internal readonly string ConnectionString = _configuration.GetConnectionString("Postgres") ?? string.Empty;
        static internal readonly string TmdbToken = _configuration["Tmdb:Token"] ?? string.Empty;
        static internal readonly string TmdbApiUrl = _configuration["Tmdb:ApiUrl"] ?? string.Empty;
        static internal readonly string GeminiApiKey = _configuration["Gemini:ApiKey"] ?? string.Empty;
        static internal readonly string GeminiApiUrl = _configuration["Gemini:ApiUrl"] ?? string.Empty;
    }
}
