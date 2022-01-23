using HRSystem.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;
using RestSharp;
using Xunit;
using System.Threading.Tasks;


namespace RestAPITests
{
    public class CandidateControllerTests
    {
        [Fact]
        public async Task Get_WithoutParameters_ReturnsAllAsync()
        {
            // Arange
            var baseUrl = "http://localhost:11635/api/";
            var client = new RestClient(baseUrl);
            var request = new RestRequest("Candidates", Method.Get);
            request.AddHeader("content-type", "application/json");
            var response = await client.ExecuteGetAsync(request);

            Assert.Equal("application/json", response.Content);
        }

        [Fact]
        public void Delete_CandidateId_ReturnsSuccess()
        {
            var configMock = new Mock<IConfiguration>();
            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var controller = new CandidatesController(configuration);

            var result = controller.Delete(1);
            JsonResult expected = new JsonResult("Deleted Successfully");
            Assert.Equal(expected.ToString(), result.ToString());
        }


    }
}
