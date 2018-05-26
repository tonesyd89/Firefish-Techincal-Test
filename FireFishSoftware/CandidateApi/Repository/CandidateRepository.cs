using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Management;
using CandidateApi.Models;

namespace CandidateApi.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly SqlConnection _sqlConnectionconnection;
        private readonly QueryBuilder _queryBuilder;
        private readonly Dictionary<string, string> _skillValues;
        /// <summary>
        /// Initializes a new instance of the <see cref="CandidateRepository" /> class.
        /// </summary>
        public CandidateRepository()

        {
            this._queryBuilder = new QueryBuilder();
            this._sqlConnectionconnection =
                new SqlConnection(
                    "Data Source = localhost; Initial Catalog = Web_API_Task; Integrated Security = True");
            this._skillValues = this.GetSkills();
        }

        private Dictionary<string, string> GetSkills()
        {
            Dictionary<string, string> skillsLDictionary = new Dictionary<string, string>();
            _sqlConnectionconnection.Open();
            SqlCommand getSkillsCommand = new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetSkillsListQuery), this._sqlConnectionconnection);
            SqlDataReader dataReader = getSkillsCommand.ExecuteReader();
            while (dataReader.Read())
            {  
               skillsLDictionary.Add(dataReader["id"].ToString(), dataReader["name"].ToString());
            }
            _sqlConnectionconnection.Close();
            return skillsLDictionary;
        }

        /// <summary>
        /// Inserts the candidateto database.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns></returns>
        public bool InsertCandidateToDatabase(Candidate candidate)
        {
            SqlCommand insertCommand = new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetInsertQuery), this._sqlConnectionconnection);

            SqlCommand getMaxCandidateId = new SqlCommand(this._queryBuilder.GetSqlCommand(Table.Candidate), this._sqlConnectionconnection);

            try
            {
                _sqlConnectionconnection.Open();
                int candidateId = (int) getMaxCandidateId.ExecuteScalar();
                candidateId++;
                candidate.CandidateId = candidateId.ToString();

                insertCommand.Parameters.AddRange(HelperClass.GetInsertCommandParameters(candidate));
                insertCommand.ExecuteNonQuery();
                _sqlConnectionconnection.Close();

                InsertCandidateSkillToDb(candidate);

                return true;
            }
            catch (Exception e)
            {
                insertCommand.Dispose();
                _sqlConnectionconnection.Close();
                return false;
            }
        }

        private void InsertCandidateSkillToDb(Candidate candidate)
        {
            SqlCommand getMaxCandidateSkillsId = new SqlCommand(this._queryBuilder.GetSqlCommand(Table.CandidateSkill), this._sqlConnectionconnection);
            SqlCommand insertSkillsCommand = new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetInsertSkillsQuery), this._sqlConnectionconnection);
            try
            {
                using (_sqlConnectionconnection)
                {
                    _sqlConnectionconnection.Open();
                    int skillId = (int) getMaxCandidateSkillsId.ExecuteScalar();

                    insertSkillsCommand.Parameters.AddRange(
                        HelperClass.GetInsertSkillsCommand());

                    foreach (var candidateSkill in candidate.Skills)
                    {
                        insertSkillsCommand.Parameters["@Id"].Value = skillId++;
                        insertSkillsCommand.Parameters["@CandidateId"].Value = candidate.CandidateId;
                        insertSkillsCommand.Parameters["@CreatedDate"].Value = DateTime.Today;
                        insertSkillsCommand.Parameters["@UpdatedDate"].Value = DateTime.Today;
                        insertSkillsCommand.Parameters["@SkillId"].Value =
                            _skillValues.FirstOrDefault(x => x.Value == candidateSkill).Key;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Gets all candidates.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Candidate> GetAllCandidates()
        {
            List<Candidate> candidates = new List<Candidate>();
            SqlCommand selectCommand = new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetSelectAllQuery), _sqlConnectionconnection);
            try
            {
                using (_sqlConnectionconnection)
                {
                    _sqlConnectionconnection.Open();
                    SqlDataReader datareader = selectCommand.ExecuteReader();

                    while (datareader.Read())
                    {
                        var candidate = HelperClass.GetCandidate(datareader);
                        candidates.Add(candidate);
                    }

                    _sqlConnectionconnection.Close();

                    List<Skill> candidateSkills = GetCandidateSkills();

                    foreach (Candidate candidate in candidates)
                    {
                        SetCandidateSkills(candidateSkills, candidate);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
            finally
            {
                _sqlConnectionconnection.Close();
            }
            return candidates;
        }

        /// <summary>
        /// Gets the candidate by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Candidate GetCandidateById(int id)

        {
            Candidate candidate = new Candidate();
            SqlCommand cmdGetCandidateById = new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetSelectByIdQuery), _sqlConnectionconnection);

            cmdGetCandidateById.Parameters.AddWithValue("@Id", id);

            try
            {
                using (_sqlConnectionconnection)
                {
                    _sqlConnectionconnection.Open();
                    SqlDataReader dataReader = cmdGetCandidateById.ExecuteReader();
                    while (dataReader.Read())
                    {
                        candidate = HelperClass.GetCandidate(dataReader);
                    }

                    _sqlConnectionconnection.Close();

                    List<Skill> candidateSkills = this.GetCandidateSkills(candidate.CandidateId);
                    SetCandidateSkills(candidateSkills, candidate);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                _sqlConnectionconnection.Close();
            }

            return candidate;
        }


     

        /// <summary>
        /// Gets the candidate skills.
        /// </summary>
        /// <returns></returns>
        public List<Skill> GetCandidateSkills()
        {
            List<Skill> candidateSkills = new List<Skill>();
            try
            {
                using (_sqlConnectionconnection)
                {
                    _sqlConnectionconnection.Open();
                    SqlDataReader dataReader = this.GetDataReader(QueryType.GetSkillsQuery);
                    while (dataReader.Read())
                    {
                        var skill = HelperClass.GetSkills(dataReader);
                        candidateSkills.Add(skill);
                    }
                }
            }
            catch (Exception e)
            {
                _sqlConnectionconnection.Close();
                Console.WriteLine(e);
                throw;
            }

            _sqlConnectionconnection.Close();
            return candidateSkills;
        }

        /// <summary>
        /// Gets the candidate skills.
        /// </summary>
        /// <param name="candidateId">The candidate identifier.</param>
        /// <returns></returns>
        public List<Skill> GetCandidateSkills(string candidateId)
        {
            List<Skill> candidateSkills = new List<Skill>();

            SqlCommand cmdGetSkills = new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetSkillsByIdQuery), _sqlConnectionconnection);

            cmdGetSkills.Parameters.AddWithValue("@Id", candidateId);
            try
            {
                using (_sqlConnectionconnection)
                {
                    _sqlConnectionconnection.Open();
                    SqlDataReader dataReader = cmdGetSkills.ExecuteReader();
                    while (dataReader.Read())
                    {
                        var skill = HelperClass.GetSkills(dataReader);
                        candidateSkills.Add(skill);
                    }
                }
            }
            catch (Exception e)
            {
                _sqlConnectionconnection.Close();
                Console.WriteLine(e);
                throw;
            }

            _sqlConnectionconnection.Close();
            return candidateSkills;
        }

        /// <summary>
        /// Sets the candidate skills.
        /// </summary>
        /// <param name="candidateSkills">The candidate skills.</param>
        /// <param name="candidate">The candidate.</param>
        private static void SetCandidateSkills(List<Skill> candidateSkills, Candidate candidate)
        {
            foreach (Skill candidateSkill in candidateSkills)
            {
                if (candidate.CandidateId == candidateSkill.CandidateId)
                {
                    candidate.Skills.Add(candidateSkill.skill);
                }
            }
        }

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="queryType">Type of the query.</param>
        /// <returns>A data Reader containing the data set for the given guery type</returns>
        private SqlDataReader GetDataReader(QueryType queryType)
        {
            SqlCommand command = new SqlCommand(this._queryBuilder.GetSqlCommand(queryType), _sqlConnectionconnection);
            return command.ExecuteReader();
        }
    }
}