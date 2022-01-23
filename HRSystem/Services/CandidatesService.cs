using HRSystem.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HRSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System;


namespace HRSystem.Services
{
    public class CandidatesService
    {
        private readonly IConfiguration m_configuration;

        public CandidatesService(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        public JsonResult Get(string partOfName = "", string skillName = "")
        {
            if (partOfName == "" && skillName == "")
            {
                return GetAll();
            }
            else if (partOfName != "" && skillName == "")
            {
                return SearchDatabaseByName(partOfName);
            }
            else if (partOfName == "" && skillName != "")
            {
                return GetCandidatesWithSkills(skillName);
            }
            else
            {
                return GetCandidatesWithNameAndSkills(partOfName, skillName);
            }
        }

        public JsonResult GetAll()
        { 
            string query = @"SELECT candidate_id, candidate_name, date_of_birth, contact_number, email from dbo.Candidates";

            DataTable dataTable = new DataTable();
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    reader = command.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }
            DataColumn dataColumn = new DataColumn();
            dataColumn.ColumnName = "skills";
            dataTable.Columns.Add(dataColumn);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                int candidate_id = int.Parse(dataRow["candidate_id"].ToString());
                dataRow["skills"] = CreateSkillString(candidate_id);
            }

            return new JsonResult(dataTable);
        }

        public JsonResult GetCandidatesWithNameAndSkills(string partOfName, string skillName)
        {
            string query = @"SELECT CANDIDATES.*
                            FROM dbo.Candidates as CANDIDATES 
                            inner join dbo.Candidate_skill AS SKILLS
                            on CANDIDATES.candidate_id = SKILLS.candidate_id
                            where SKILLS.skill_id in
                            (
                                select skill_id from SKILLS where skill_name = @skillName and CANDIDATES.candidate_name LIKE '%'+@partOfName+'%'
                            )";
            
            DataTable dataTable = new DataTable();
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@skillName", skillName);
                    command.Parameters.AddWithValue("@partOfName", partOfName);
                    reader = command.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            DataColumn dataColumn = new DataColumn();
            dataColumn.ColumnName = "skills";
            dataTable.Columns.Add(dataColumn);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                int candidate_id = int.Parse(dataRow["candidate_id"].ToString());
                dataRow["skills"] = CreateSkillString(candidate_id);
            }

            return new JsonResult(dataTable);
        }

        public JsonResult SearchDatabaseByName(string partOfName)
        {
            string query = @"SELECT * from dbo.Candidates WHERE candidate_name LIKE '%'+@partOfName+'%'";

            DataTable dataTable = new DataTable();
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@partOfName", partOfName);
                    reader = command.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            DataColumn dataColumn = new DataColumn();
            dataColumn.ColumnName = "skills";
            dataTable.Columns.Add(dataColumn);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                int candidate_id = int.Parse(dataRow["candidate_id"].ToString());
                dataRow["skills"] = CreateSkillString(candidate_id);
            }

            return new JsonResult(dataTable);
        }

        public JsonResult GetCandidatesWithSkills(string skillName = "")
        {
            string query;
            if (skillName == "")
            {
                query = @"SELECT candidate_id, candidate_name, date_of_birth, contact_number, email from dbo.Candidates";
            }
            else
            {
                query = @"SELECT CANDIDATES.*
                            FROM dbo.Candidates as CANDIDATES 
                            inner join dbo.Candidate_skill AS SKILLS
                            on CANDIDATES.candidate_id = SKILLS.candidate_id
                            where SKILLS.skill_id in
                            (
                                select skill_id from SKILLS where skill_name = @skillName
                            )";
            }

            DataTable dataTable = new DataTable();
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@skillName", skillName);
                    reader = command.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                    con.Close();
                }
            }

            DataColumn dataColumn = new DataColumn();
            dataColumn.ColumnName = "skills";
            dataTable.Columns.Add(dataColumn);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                int candidate_id = int.Parse(dataRow["candidate_id"].ToString());
                dataRow["skills"] = CreateSkillString(candidate_id);
            }

            return new JsonResult(dataTable);
        }

        public JsonResult AddCandidateWithSkills(Candidate candidate, string skills)
        {
            string candidateName = candidate.Name;
            DateTime date = candidate.DateOfBirth;
            string contactNumber = candidate.ContactNumber;
            string email = candidate.Email;

            if (IsInTable(email)) return new JsonResult("User already in table");

            string query = @"insert into dbo.Candidates
                                values (@candidate_name, @date_of_birth, @contact_number, @email);
                            select SCOPE_IDENTITY()";


            int insertedId = 0;

            string dataSource = m_configuration.GetConnectionString("HRSystemApp");
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@candidate_name", candidateName);
                    command.Parameters.AddWithValue("@date_of_birth", date);
                    command.Parameters.AddWithValue("@contact_number", contactNumber);
                    command.Parameters.AddWithValue("@email", email);
                    object returnObj = command.ExecuteScalar();
                    if (returnObj != null)
                    {
                        int.TryParse(returnObj.ToString(), out insertedId);
                    }
                    command.Dispose();
                }
                con.Close();
            }
            AddSkillsForCandidate(insertedId, skills);

            return new JsonResult("Added successfully");
        }

        public JsonResult UpdateCandidateWithSkill(Candidate candidate, string skill)
        {
           AddSkillsForCandidate(candidate.Id, skill);

           return new JsonResult("Added successfully");
        }

        public JsonResult DeleteSkillFromCandidate(int candidateId, string skill)
        {
            SkillsServices skillsService = new SkillsServices(m_configuration);
            int skillId = skillsService.GetSkillId(skill);
            CandidateSkillServices candidateSkillServices = new CandidateSkillServices(m_configuration);
            return candidateSkillServices.Delete(candidateId, skillId);
        }

        public JsonResult DeleteCandidate(int id)
        {
            string findQuery = @"Select * from dbo.Skills where skill_id = @skillId";
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");
            SqlDataReader findReader = null;
            bool message;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(findQuery, con))
                {
                    command.Parameters.AddWithValue("@skillId", id);
                    findReader = command.ExecuteReader();
                    message = findReader.Read();
                    findReader.Close();
                    con.Close();
                }
            }
            if (message != true) throw new Exception("No such candidate in Database");

            string query = @"delete from dbo.Candidates
                           where candidate_id = @candidateId";
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@candidateId", id);
                    reader = command.ExecuteReader();
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        public bool IsInTable(string email)
        {
            string query = @"SELECT * FROM dbo.Candidates
                            WHERE email=@email";

            bool inTable = false;
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");

            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@email", email);
                    object o = command.ExecuteScalar();
                    inTable = ((o == null) || (o == DBNull.Value)) ? false :
                                                (int)o == 0 ? false : true;
                    command.Dispose();
                    con.Close();
                }
            }
            return inTable;
        }

        public void AddSkillsForCandidate(int candidateId, string skills)
        {

            if (skills != null)
            {
                string[] skill = skills.Split(" ");
                foreach (var it in skill)
                {
                    SkillsServices skillsService = new SkillsServices(m_configuration);

                    skillsService.Post(it);
                    int skillId = skillsService.GetSkillId(it);

                    CandidateSkillServices candidateSkillServices = new CandidateSkillServices(m_configuration);
                    candidateSkillServices.Post(candidateId, skillId);
                }
            }
        }

        //protected string AddSkill(int candidateId, string skill)
        //{

        //    if (skill != null)
        //    {
        //        SkillsServices skillsService = new SkillsServices(m_configuration);

        //        skillsService.Post(skill);
        //        int skillId = skillsService.GetSkillId(skill);

        //        CandidateSkillServices candidateSkillServices = new CandidateSkillServices(m_configuration);
        //        candidateSkillServices.Post(candidateId, skillId);
        //    }
        //    return "Skills refreshed";
        //}

        protected string CreateSkillString(int candidateId)
        {
            CandidateSkillServices candidateSkillServices = new CandidateSkillServices(m_configuration);
            SkillsServices skillsService = new SkillsServices(m_configuration);
            string skillList = skillsService.GetSkillsForCandidate(candidateId);
            return skillList;
        }

    }
}
