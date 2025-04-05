using LocalRNC.Controllers;
using LocalRNC.Data;
using LocalRNC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LocalRNC.Tests
{
    [TestClass]
    public sealed class CompanyControllerUnitTest
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new ApplicationDbContext(options);
            context.companies.Add(new Company { Name = "Company1", RNC = "130403899", Created_at = DateTime.UtcNow });
            context.companies.Add(new Company { Name = "Company2", RNC = "130403898", Created_at = DateTime.UtcNow });
            context.SaveChanges();

            return context;
        }

        [TestMethod]
        public async Task GetCompanyById_ReturnsCompany_WhenExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new CompanyController(context);

            // Act
            var result = await controller.Get(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            
            var company = okResult.Value as Company;
            Assert.IsNotNull(company);
            Assert.AreEqual(1, company.Id);
            Assert.AreEqual("Company1", company.Name);
            Assert.AreEqual("130403899", company.RNC);
        }

        [TestMethod]
        public async Task GetAllCompanies_ReturnsCompanies_WhenExist()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new CompanyController(context);

            // Act
            var result = await controller.Get();  // The method returns ActionResult<IEnumerable<Company>>

            // Assert
            var type = result.Result.GetType();
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var companies = okResult.Value as List<Company>;
            Assert.IsNotNull(companies);
            Assert.AreEqual(2, companies.Count);  // We added two companies in memory
            Assert.AreEqual("Company1", companies[0].Name);
            Assert.AreEqual("Company2", companies[1].Name);
        }
    }
}
