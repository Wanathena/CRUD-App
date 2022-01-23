using HRSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using HRSystem.Services;

namespace HRSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private CandidatesService m_candidatesService;

        public CandidatesController(IConfiguration configuration)
        {
            m_candidatesService = new CandidatesService(configuration);
        }

        [HttpGet]
        public JsonResult Get(string partOfName = "", string skillName = "")
        {
            return m_candidatesService.Get(partOfName, skillName);
        }

        [HttpPost("{skills}")]
        public JsonResult Post(Candidate candidate, string skills)
        {
            return m_candidatesService.AddCandidateWithSkills(candidate, skills);
        }

        [HttpPut("{skills}")]
        public JsonResult Put(Candidate candidate, string skills)
        {
            return m_candidatesService.UpdateCandidateWithSkill(candidate, skills);
        }

        [HttpPut("{candidateId}/{skillName}")]
        public JsonResult Put(int candidateId, string skillName)
        {
            return m_candidatesService.DeleteSkillFromCandidate(candidateId, skillName);
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            return m_candidatesService.DeleteCandidate(id);
        }
    }
}
