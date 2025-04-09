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
        public ApplicationDbContext _context;
        public CompanyControllerUnitTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _context.companies.AddRange(
                new Company { Name = "Company1", RNC = "130403899", Created_at = DateTime.UtcNow },
                new Company { Name = "Company2", RNC = "130403898", Created_at = DateTime.UtcNow }
            );

            _context.SaveChanges();
        }


        [TestMethod]
        public async Task GetCompany_ReturnsCompanyById_WhenExists()
        {
            // Arrange
            var controller = new CompanyController(_context);

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
        public async Task GetCompany_ReturnsAllCompanies_WhenExist()
        {
            // Arrange
            var controller = new CompanyController(_context);

            // Act
            var result = await controller.Get();

            // Assert
            var companies = result.Value as List<Company>;
            Assert.IsNotNull(companies);
            Assert.AreEqual(2, companies.Count);
            Assert.AreEqual("Company1", companies[0].Name);
            Assert.AreEqual("Company2", companies[1].Name);
        }

        [TestMethod]
        public async Task CreateCompany_AddCompanyToDatabase()
        {
            // Arrange
            var controller = new CompanyController(_context);
            var newCompany = new Company
            {
                Name = "New Company",
                RNC = "130403897",
                Created_at = DateTime.UtcNow
            };

            // Act
            var result = await controller.Post(newCompany);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);

            var company = createdResult.Value as Company;
            Assert.IsNotNull(company);
            Assert.AreEqual("New Company", company.Name);
            Assert.AreEqual("130403897", company.RNC);

            var dbCompany = await _context.companies.FindAsync(company.Id);
            Assert.IsNotNull(dbCompany);
        }

        [TestMethod]
        public async Task UpdateCompany_UpdatesExistingCompany()
        {
            // Arrange
            var controller = new CompanyController(_context);
            var companyToUpdate = await _context.companies.FirstAsync();
            companyToUpdate.Name = "Updated Company";

            // Act
            var result = await controller.Put(companyToUpdate.Id, companyToUpdate);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NoContentResult));

            var updatedCompany = await _context.companies.FindAsync(companyToUpdate.Id);
            Assert.AreEqual("Updated Company", updatedCompany.Name);
        }

        [TestMethod]
        public async Task DeleteCompany_RemovesCompanyFromDatabase()
        {
            // Arrange
            var controller = new CompanyController(_context);
            var companyIdToDelete = 1;

            // Act
            var result = await controller.Delete(companyIdToDelete);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NoContentResult));

            var deletedCompany = await _context.companies.FindAsync(companyIdToDelete);
            Assert.IsNull(deletedCompany);
        }
    }
}
