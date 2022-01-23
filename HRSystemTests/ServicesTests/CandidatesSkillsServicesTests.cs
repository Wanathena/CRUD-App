using HRSystem.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HRSystemTests.ServicesTests
{
    public class CandidatesSkillsServicesTests
    {

        [Fact]
        public void Delete_CandidateSkills_ExceptionThrown()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new CandidateSkillServices(configuration);
            
           
            var result = service.Delete(2000, 2000).Value.ToString();
            
           
            Assert.Equal("No such element in Database", result);
          
        }

        [Fact]
        public void Post_CandidateIdSkillId_AlreadyInTable()
        {
            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new CandidateSkillServices(configuration);

           
                var result = service.Post(5, 1).Value.ToString();
            
                Assert.Equal("Already in Table", result);
            
        }

        [Fact]
        public void Post_CandidateIdSkillId_AddedSuccessfully()
        {
            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new CandidateSkillServices(configuration);

            service.Delete(5, 24);

            var result = service.Post(5, 24).Value.ToString();

            Assert.Equal("Added successfully", result);
           
            service.Delete(5, 24);
        }


        [Fact]
        public void IsInTable_ReturnTrue()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new CandidateSkillServices(configuration);

            var result = service.IsInTable(5, 1);
            var expected = true;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsNotInTable_ReturnTrue()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new CandidateSkillServices(configuration);

            var result = service.IsInTable(5, 40);
            var expected = false;

            Assert.Equal(expected, result);
        }
    }
}
