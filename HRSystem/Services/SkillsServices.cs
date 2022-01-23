using HRSystem.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HRSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System;

namespace HRSystem.Services
{
    public class SkillsServices
    {
        private readonly IConfiguration m_configuration;

        public SkillsServices(IConfiguration config)
        {
            m_configuration = config;
        }

        public JsonResult Get()
        {
            string query = @"SELECT skill_id, skill_name from dbo.Skills";

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

        //public JsonResult Get(string skillName)
        //{
        //    string query = @"SELECT skill_id from dbo.Skills where skill_name = @skillName";
        //    DataTable dataTable = new DataTable();
        //    string dataSource = m_configuration.GetConnectionString("HRSystemApp");
        //    SqlDataReader reader = null;
        //    using (SqlConnection con = new SqlConnection(dataSource))
        //    {
        //        con.Open();
        //        using (SqlCommand command = new SqlCommand(query, con))
        //        {
        //            command.Parameters.AddWithValue("@skillName", skillName);
        //            reader = command.ExecuteReader();
        //            dataTable.Load(reader);
        //            reader.Close();
        //            con.Close();
        //        }
        //    }
        //    return new JsonResult(dataTable);
        //}

        public JsonResult Post(string skillName)
        {

            if (IsInTable(skillName)) return new JsonResult("Already in database");

            string query = @"insert into dbo.Skills
                            values (@skill_name)";
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@skill_name", skillName);
                    reader = command.ExecuteReader();
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult("Added successfully");
        }

        public JsonResult Delete(int id)
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
            if (message != true) return new JsonResult("No such element in Database");

            string query = @"delete from dbo.Skills
                           where skill_id = @skillId";
            SqlDataReader reader = null;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@skillId", id);
                    reader = command.ExecuteReader();
                    reader.Close();
                    con.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        public bool IsInTable(string skill)
        {
            string query = @"SELECT * FROM dbo.Skills
                            WHERE skill_name=@skill";

            bool inTable = false;
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");

            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@skill", skill);
                    object o = command.ExecuteScalar();
                    inTable = ((o == null) || (o == DBNull.Value)) ? false :
                        (int)o == 0 ? false : true;
                    command.Dispose();
                    con.Close();
                }
            }
            return inTable;
        }

        public int GetSkillId(string skillName)
        {
            string query = @"SELECT skill_id FROM dbo.Skills WHERE skill_name=@skillName";
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");

            int id = 0;

            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@skillName", skillName);
                    object returnObj = command.ExecuteScalar();
                    if (returnObj != null)
                    {
                        int.TryParse(returnObj.ToString(), out id);
                    }
                }
            }
            return id;
        }

        public string GetSkillsForCandidate(int id)
        {
            string query = @"select skill_name from dbo.Skills where skill_id = @skillId";
            string dataSource = m_configuration.GetConnectionString("HRSystemApp");
            string returnString = "";
            CandidateSkillServices candidateSkillServices = new CandidateSkillServices(m_configuration);
            System.Collections.Generic.List<int> json = candidateSkillServices.getSkillsForCandidate(id);

            foreach (var it in json)
            {
                using (SqlConnection con = new SqlConnection(dataSource))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@skillId", it);
                        object returnObj = cmd.ExecuteScalar();
                        returnString += returnObj.ToString();
                        
                        returnString += " ";
                    }
                    con.Close();
                }
            }
            returnString.TrimEnd();
            return returnString;
        }

        //internal string getSkill(int candidateId)
        //{
        //    string query = @"select skill_name from dbo.Skills
        //                    inner join dbo.Candidate_skill
        //                    on  dbo.Candidate_skill.candidate_id = @candidateId";

        //    string dataSource = m_configuration.GetConnectionString("HRSystemApp");

        //    string skills = "";

        //    using (SqlConnection con = new SqlConnection(dataSource))
        //    {
        //        con.Open();
        //        using (SqlCommand command = new SqlCommand(query, con))
        //        {
        //            command.Parameters.AddWithValue("@candidateId", candidateId);
        //            object returnObj = command.ExecuteScalar();
        //            if (returnObj != null)
        //            {
        //                skills = returnObj.ToString();
        //            }
        //        }
        //    }
        //    return skills;
        //}
    }
}
