using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using CandidateApi.Models;

namespace CandidateApi.Repository
{
    public class HelperClass
    {
        public static SqlParameter[] GetInsertCommandParameters(Candidate candidate)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@Id", candidate.CandidateId),
                new SqlParameter("@FirstName", candidate.FirstName),
                new SqlParameter("@Surname", candidate.Surname),
                new SqlParameter("@DateOfBirth", candidate.DateOfBirth),
                new SqlParameter("@Address1", candidate.Address1),
                new SqlParameter("@Town", candidate.Town),
                new SqlParameter("@Country", candidate.Country),
                new SqlParameter("@Postcode", candidate.Postcode),
                new SqlParameter("@PhoneHome", candidate.PhoneHome),
                new SqlParameter("@PhoneMobile", candidate.PhoneMobile),
                new SqlParameter("@PhoneWork", candidate.PhoneWork),
                new SqlParameter("@CreatedDate", candidate.CreatedDate),
                new SqlParameter("@UpdatedDate", candidate.UpdatedDate),
            };
        }

        public static Candidate GetCandidate(SqlDataReader datareader)
        {
            Candidate candidate = new Candidate()
            {
                CandidateId = datareader["Id"].ToString(),
                FirstName = datareader["FirstName"].ToString(),
                Surname = datareader["Surname"].ToString(),
                DateOfBirth = Convert.ToDateTime(datareader["DateOfbirth"].ToString()),
                Address1 = datareader["Address1"].ToString(),
                Town = datareader["Town"].ToString(),
                Country = datareader["Country"].ToString(),
                Postcode = datareader["PostCode"].ToString(),
                PhoneHome = datareader["PhoneHome"].ToString(),
                PhoneMobile = datareader["PhoneMobile"].ToString(),
                PhoneWork = datareader["PhoneWork"].ToString(),
                UpdatedDate = Convert.ToDateTime(datareader["UpdatedDate"].ToString()),
                CreatedDate = Convert.ToDateTime(datareader["CreatedDate"].ToString()),
            };
            return candidate;
        }

        public static Skill GetSkills(SqlDataReader reader)
        {
            Skill skill = new Skill()
            {
                CandidateId = reader["Id"].ToString(),
                skill = reader["name"].ToString()
            };
            return skill;
        }

        /// <summary>
        /// Gets the insert skills command.
        /// </summary>
        /// <returns></returns>
        public static SqlParameter[] GetInsertSkillsCommand()
        {
            return new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.VarChar),
                new SqlParameter("@CandidateId", SqlDbType.VarChar),
                new SqlParameter("@CreatedDate", SqlDbType.DateTime),
                new SqlParameter("@UpdatedDate", SqlDbType.DateTime),
                new SqlParameter("@SkillId", SqlDbType.VarChar)
            };
        }
    }
}