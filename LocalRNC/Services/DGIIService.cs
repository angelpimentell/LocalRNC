using LocalRNC.Data;
using LocalRNC.Models;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace LocalRNC.Services
{
    public class DGIIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DGIIService> _logger;
        private readonly string _basePath;
        private readonly string _rncZipPath;
        private readonly string _rncFolderPath;
        private readonly string _rncTxtPath;
        private readonly ApplicationDbContext _dbContext;   
        private readonly char sep = Path.DirectorySeparatorChar;

        public DGIIService(ApplicationDbContext dbContext)
        {
            _httpClient = new HttpClient();
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<DGIIService>();

            _basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            _rncZipPath = _basePath + $"{sep}rnc.zip";
            _rncFolderPath = _basePath + $"{sep}rnc_extracted";
            _rncTxtPath = _rncFolderPath + $"{sep}TMP{sep}DGII_RNC.txt";
            _dbContext = dbContext;
        }

        private bool ExistsDgiiFileValid()
        {
            if (File.Exists(_rncZipPath))
            {
                return true;
            }

            return false;
        }

        public async Task DownloadFileAsync()
        {
            const string url = "https://www.dgii.gov.do/app/WebApps/Consultas/RNC/DGII_RNC.zip";

            if (!ExistsDgiiFileValid())
            {
                _logger.LogInformation("Downlading file from DGII");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(_rncZipPath, fileBytes);

                _logger.LogInformation("File downloaded");
            }

        }

        public async Task UpdateDB()
        {
            _logger.LogInformation("Unziping file");

            if (Directory.Exists(_rncFolderPath))
            {
                Directory.Delete(_rncFolderPath, true);
            }

            ZipFile.ExtractToDirectory( _rncZipPath, _rncFolderPath);
            _logger.LogInformation("Unziped file");

            string[] lines = File.ReadAllLines(_rncTxtPath);
            _logger.LogInformation("File contents (line by line):");

            foreach (var line in lines)
            {
                string[] data = line.Split('|');
                string rnc = data[0];
                string name = data[1];
                string description = data[3];

                var company = await _dbContext.companies.FirstOrDefaultAsync(r => r.RNC == rnc);

                if (company != null)
                {
                    company.Name = name;
                    company.Description = description;
                }
                else
                {
                    var newCompany = new Company
                    {
                        RNC = rnc,
                        Name = name,
                        Description = description,
                        Created_at = DateTime.UtcNow
                    };
                    await _dbContext.companies.AddAsync(newCompany);
                }

                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
