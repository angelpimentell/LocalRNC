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
            context.companies.Add(new Company { Id = 1, Name = "Test Company", RNC = "130403899", Created_at = DateTime.UtcNow });
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
            Assert.AreEqual("Test Company", company.Name);
            Assert.AreEqual("130403899", company.RNC);
        }
    }
}
