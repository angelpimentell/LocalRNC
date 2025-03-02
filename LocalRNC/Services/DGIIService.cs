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
        private readonly string _basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        private readonly string _zipFilePath;
        private readonly string _extractedPath;
        private readonly string _rncPath;
        private readonly ApplicationDbContext _dbContext;   

        public DGIIService(ApplicationDbContext dbContext)
        {
            this._httpClient = new HttpClient();
            this._logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<DGIIService>();

            this._zipFilePath = this._basePath + "\\rnc.zip";
            this._extractedPath = this._basePath + "\\rnc_extracted";
            this._rncPath = this._extractedPath + "\\TMP\\DGII_RNC.txt";
            this._dbContext = dbContext;
        }

        private bool ExistsDgiiFileValid()
        {
            if (File.Exists(this._zipFilePath))
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
                await File.WriteAllBytesAsync(this._zipFilePath, fileBytes);

                _logger.LogInformation("File downloaded");
            }

        }

        public async void UpdateDB()
        {
            _logger.LogInformation("Unziping file");
            ZipFile.ExtractToDirectory(this._zipFilePath, this._extractedPath);
            _logger.LogInformation("Unziped file");

            string[] lines = File.ReadAllLines(this._rncPath);
            _logger.LogInformation("File contents (line by line):");

            foreach (var line in lines)
            {
                string[] data = line.Split('|');
                string rnc = data[0];
                string name = data[1];
                string description = data[3];

                var connectionString = _dbContext.Database.GetDbConnection().ConnectionString;

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
                        Created_at = DateTime.Now
                    };
                    await this._dbContext.companies.AddAsync(newCompany);
                }

                await this._dbContext.SaveChangesAsync();
            }


        }

    }
}
