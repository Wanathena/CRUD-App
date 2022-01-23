using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RestSharp;
using System.Net;
using HRSystem.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;

namespace HRSystemTests.RestAPITests
{
    public class SkillsControllerTests
    {
        [Fact]
        public async Task Get_WithoutParameters_ReturnsAllAsync()
        {
            // Arange
            IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
            IPEndPoint ip = new IPEndPoint(ipAddress, 11635);
            Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            var baseUrl = "http://127.0.0.1:11635/";
            
            serverSocket.Bind(ip);
            
          
            var client = new RestClient(baseUrl);
            var builder = new ConfigurationBuilder();
            const string settingsFile = "appsettings.json";
            builder.AddJsonFile(settingsFile);
            var configuration = builder.Build();
            client.AddDefaultHeader("Connection", "Keep-Alive");
            SkillsServices skillsServices = new SkillsServices(configuration);
            skillsServices.GetSkillId("dsafdsfasvewarvfsda");
            skillsServices.Delete(skillsServices.GetSkillId("dsafdsfasvewarvfsda"));
            var request = new RestRequest("api/"+ "Skills/skillName?skillName="+"dsafdsfasvewarvfsda", Method.Post);
            skillsServices.GetSkillId("dsafdsfasvewarvfsda");
            skillsServices.Delete(skillsServices.GetSkillId("dsafdsfasvewarvfsda"));

            var response = await client.ExecutePostAsync<ResponseStatus>(request);

            Assert.Equal("Added Successfully", response.Content.ToString());
        }
    }
}
