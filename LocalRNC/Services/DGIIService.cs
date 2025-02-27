namespace LocalRNC.Services
{
    public class DGIIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DGIIService> _logger;

        public DGIIService()
        {
            this._httpClient = new HttpClient();
            this._logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<DGIIService>();
        }

        private static bool ExistsDgiiFileValid()
        {
            return true;
        }

        public async Task DownloadFileAsync(string filePath)
        {
            const string url = "https://www.dgii.gov.do/app/WebApps/Consultas/RNC/DGII_RNC.zip";

            if (ExistsDgiiFileValid())
            {
                _logger.LogInformation("Downlading file from DGII");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(filePath, fileBytes);

                _logger.LogInformation("File downloaded");
            }

        }

    }
}
