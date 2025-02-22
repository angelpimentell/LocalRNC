namespace LocalRNC.Services
{
    public class DGIIService(HttpClient httpClient, ILogger<DGIIService> logger)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<DGIIService> _logger = logger;

        public async Task DownloadFileAsync(string filePath)
        {
            const string url = "https://www.dgii.gov.do/app/WebApps/Consultas/RNC/DGII_RNC.zip";

            try
            {
                _logger.LogInformation("Downlading file from DGII");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(filePath, fileBytes);

                _logger.LogInformation("File downloaded");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downlading file");
            }
        }

    }
}
