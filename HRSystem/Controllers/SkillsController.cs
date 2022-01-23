using HRSystem.Models;
using HRSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HRSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController
    {
        private SkillsServices m_skillsService;

        public SkillsController(IConfiguration config)
        {
            m_skillsService = new SkillsServices(config);
        }

        [HttpGet]
        public JsonResult Get()
        {
            return m_skillsService.Get();
        }

        [HttpPost("skillName")]
        public JsonResult Post(string skillName)
        {
            return m_skillsService.Post(skillName);
        }
    }
}
