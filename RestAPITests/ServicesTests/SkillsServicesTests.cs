using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRSystem.Services;
using Microsoft.Extensions.Configuration;
using Xunit;


namespace HRSystemTests.ServicesTests
{
    public class SkillsServicesTests
    {

        [Fact]
        public void IsInTable_ReturnTrue()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new SkillsServices(configuration);

            var result = service.IsInTable("CPP");
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

            var service = new SkillsServices(configuration);

            var result = service.IsInTable("dsfgujhsdfakjnv");
            var expected = false;

            Assert.Equal(expected, result);
        }


        [Fact]
        public void GetSkillsForCandidate_CandidateId_ReturnsStringWithSkills()
        {
            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new SkillsServices(configuration);
            var result = service.GetSkillsForCandidate(39);
            var expected = "C# React JavaScript SQLServer CPP QML Qt ";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetSkillsForCandidate_CandidateId_ReturnsEmptyString()
        {
            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new SkillsServices(configuration);
            var result = service.GetSkillsForCandidate(1002);
            var expected = "";

            Assert.Equal(expected, result);
        }


        [Fact]
        public void GetSkillId_SendName_ReturnsId()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new SkillsServices(configuration);

            var result = service.GetSkillId("C#");
            var expected = 1;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetSkillId_SendName_ReturnsError()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new SkillsServices(configuration);

            var result = service.GetSkillId("Cfdsafasdfasdfas#");
            var expected = 0;

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Delete_SkillId_Successful()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new SkillsServices(configuration);

            service.Post("fdsfsdafdsfsdfsdfsdafsdafdsvcwq");
            int id = service.GetSkillId("fdsfsdafdsfsdfsdfsdafsdafdsvcwq");
            var result = service.Delete(id).Value.ToString();
            var expected = "Deleted Successfully";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Delete_SkillId_ExceptionThrown()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new SkillsServices(configuration);

            int id = service.GetSkillId("agasdfasdfgvbsdkafads");

            try
            {
                var result = service.Delete(id).Value.ToString();
            }
            catch (Exception ex)
            {
                Assert.Equal("No such element in Database", ex.Message);
            }
        }

        [Fact]
        public void Post_SkillName_ExceptionThrown()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new SkillsServices(configuration);


            try
            {
                service.Post("C#");
            }
            catch (Exception ex)
            {
                Assert.Equal("Already in database", ex.Message);
            }
        }

        [Fact]
        public void Post_Skill_Successful()
        {
            // Arange

            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();

            var service = new SkillsServices(configuration);
            var id = 0;
            id = service.GetSkillId("dsfaasdvsafwqvsaq");
            if(id != 0)
                service.Delete(id);
            var result = service.Post("dsfaasdvsafwqvsaq").Value.ToString();
           
            var expected = "Added successfully";

            Assert.Equal(expected, result);
        }
    }
}
