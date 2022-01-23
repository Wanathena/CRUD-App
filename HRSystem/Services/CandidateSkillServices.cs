using HRSystem.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HRSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System;

namespace HRSystem.Services
{
    public class CandidateSkillServices
    {
        private readonly IConfiguration m_configuration;

        public CandidateSkillServices(IConfiguration config)
        {
            m_configuration = config;
        }
        public JsonResult Get()
        {
            string query = @"SELECT candidate_id, skill_id from dbo.Candidate_skill";

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
            return new JsonResult(dataTable);
        }

        public JsonResult Post(int candidateId, int skillId)
        {

            if (!IsInTable(candidateId, skillId))
            {
                string query = @"insert into dbo.Candidate_skill
                            (candidate_id, skill_id)
                            values (@candidateId, @skillId)";

                string dataSource = m_configuration.GetConnectionString("HRSystemApp");
                SqlDataReader reader = null;
                using (SqlConnection con = new SqlConnection(dataSource))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@candidateId", candidateId);
                        command.Parameters.AddWithValue("@skillId", skillId);
                        reader = command.ExecuteReader();
                        reader.Close();
                        con.Close();
                    }
                }
                return new JsonResult("Added successfully");
            }

            return new JsonResult("Already in Table");
        }

        public JsonResult Delete(int candidateId, int skillId)
        {

            string findQuery = @"Select * from dbo.Candidate_skill where skill_id = @skillId and candidate_id = @candidateId";
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");
            SqlDataReader findReader = null;
            bool message;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(findQuery, con))
                {
                    command.Parameters.AddWithValue("@skillId", skillId);
                    command.Parameters.AddWithValue("@candidateId", candidateId);
                    findReader = command.ExecuteReader();
                    message = findReader.Read();
                    findReader.Close();
                    con.Close();
                }
            }
            if (message != true) return new JsonResult("No such element in Database");

            string query = @"delete from dbo.Candidate_skill
                           where 
                           candidate_id = @candidateId AND skill_id = @skillId";
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@candidateId", candidateId);
                    command.Parameters.AddWithValue("@skillId", skillId);
                    reader = command.ExecuteReader();
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        public bool IsInTable(int candidateId, int skillId)
        {
            string query = @"SELECT * FROM dbo.Candidate_skill
                            WHERE candidate_id = @candidateId AND skill_id = @skillId";

            bool inTable = false;

            string dataSource = m_configuration.GetConnectionString("HRSystemApp");

            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@candidateId", candidateId);
                    command.Parameters.AddWithValue("skillId", skillId);
                    object o = command.ExecuteScalar();
                    inTable = ((o == null) || (o == DBNull.Value)) ? false :
                                                (int)o == 0 ? false : true;
                    command.Dispose();
                    con.Close();
                }
            }
            return inTable;
        }

        internal System.Collections.Generic.List<int> getSkillsForCandidate(int id)
        {
            string query = @"select skill_id from dbo.Candidate_skill where candidate_id = @id";

            string dataSource = m_configuration.GetConnectionString("HRSystemApp");

            DataTable dataTable = new DataTable();
            SqlDataReader sqlDataReader = null;
            var skills = new System.Collections.Generic.List<int>();

            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        skills.Add(Convert.ToInt32(sqlDataReader[0]));
                    }
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                }
                con.Close();
            }

            return skills;
        }
    }
}
