using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using CandidateApi.Models;

namespace CandidateApi.Repository
{
    public class HelperClass
    {
        public static SqlParameter[] GetCandidateParameters(Candidate candidate)
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
                new SqlParameter("@CreatedDate", DateTime.Now),
                new SqlParameter("@UpdatedDate", DateTime.Now),
            };
        }

        public static SqlParameter[] GetCandidateUpdateParameters(Candidate candidate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (candidate.FirstName != null)
            {
                parameters.Add(new SqlParameter("@FirstName", candidate.FirstName));
            }

            if (candidate.Surname != null)
            {
                parameters.Add(new SqlParameter("@Surname", candidate.Surname));
            }

            if (candidate.DateOfBirth != default(DateTime))
            {
                parameters.Add(new SqlParameter("@DateOfBirth", candidate.DateOfBirth));
            }

            if (candidate.Address1 != null)
            {
                parameters.Add(new SqlParameter("@Address1", candidate.Address1));
            }

            if (candidate.Town != null)
            {
                parameters.Add(new SqlParameter("@Town", candidate.Town));
            }

            if (candidate.Country != null)
            {
                parameters.Add(new SqlParameter("@Country", candidate.Country));
            }

            if (candidate.Postcode != null)
            {
                parameters.Add(new SqlParameter("@Postcode", candidate.Postcode));
            }

            if (candidate.PhoneHome != null)
            {
                parameters.Add(new SqlParameter("@PhoneHome", candidate.PhoneHome));
            }

            if (candidate.PhoneMobile != null)
            {
                parameters.Add(new SqlParameter("@PhoneMobile", candidate.PhoneMobile));
            }

            if (candidate.PhoneWork != null)
            {
                parameters.Add(new SqlParameter("@PhoneWork", candidate.PhoneWork));
            }

            parameters.Add(new SqlParameter("@Id", candidate.CandidateId));
            parameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
            return parameters.ToArray();
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
        public static SqlParameter[] GetInsertSkillsParameters()
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