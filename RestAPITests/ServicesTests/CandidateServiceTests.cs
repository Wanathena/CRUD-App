using HRSystem.Models;
using HRSystem.Services;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace HRSystemTests.ServicesTests
{

    public class CandidateServiceTests
    {

        [Fact]
        public void IsNotInTable_ReturnTrue()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new CandidatesService(configuration);

            var result = service.IsInTable("email@email.com");
            var expected = false;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsInTable_ReturnTrue()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new CandidatesService(configuration);

            var result = service.IsInTable("email@gmail.com");
            var expected = true;

            Assert.Equal(expected, result);
        }


    }
}
