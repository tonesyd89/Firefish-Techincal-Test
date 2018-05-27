using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using CandidateApi.Models;

namespace CandidateApi.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["CandidateDbConnectionString"].ConnectionString;
        private readonly QueryBuilder _queryBuilder;
        private readonly Dictionary<string, string> _skillValues;
        /// <summary>
        /// Initializes a new instance of the <see cref="CandidateRepository" /> class.
        /// </summary>
        public CandidateRepository()

        {
            this._queryBuilder = new QueryBuilder();
            this._skillValues = this.GetSkills();
        }

        private Dictionary<string, string> GetSkills()
        {
            Dictionary<string, string> skillsDictionary = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand getSkillsCommand =
                    new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetSkillsListQuery),
                        connection))
                {
                    using (SqlDataReader dataReader = getSkillsCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            skillsDictionary.Add(dataReader["id"].ToString(), dataReader["name"].ToString());
                        }
                    }
                }
                return skillsDictionary;
            }
        }

        /// <summary>
        /// Inserts the candidateto database.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns></returns>
        public bool InsertCandidateToDatabase(Candidate candidate)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand getMaxCandidateId =
                        new SqlCommand(this._queryBuilder.GetSqlCommand(Table.Candidate), connection))
                    {
                        int candidateId = (int) getMaxCandidateId.ExecuteScalar();
                        candidateId++;
                        candidate.CandidateId = candidateId.ToString();

                    }

                    using (SqlCommand insertCommand =
                        new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetInsertQuery), connection))
                    {
                        insertCommand.Parameters.AddRange(HelperClass.GetInsertCommandParameters(candidate));
                        insertCommand.ExecuteNonQuery();
                    }

                    InsertCandidateSkillToDb(candidate);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void InsertCandidateSkillToDb(Candidate candidate)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    int skillId;
                    using (SqlCommand getMaxCandidateSkillsId =
                        new SqlCommand(this._queryBuilder.GetSqlCommand(Table.CandidateSkill), connection))
                    {
                         skillId = (int) getMaxCandidateSkillsId.ExecuteScalar();
                    }

                    foreach (var candidateSkill in candidate.Skills)
                        {
                            using (SqlCommand insertSkillsCommand =
                                new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetInsertSkillsQuery),
                                    connection))
                            {
                                insertSkillsCommand.Parameters.AddRange(
                                    HelperClass.GetInsertSkillsCommand());
                                skillId++;
                                insertSkillsCommand.Parameters["@Id"].Value = skillId;
                                insertSkillsCommand.Parameters["@CandidateId"].Value = candidate.CandidateId;
                                insertSkillsCommand.Parameters["@CreatedDate"].Value = DateTime.Today.ToLocalTime();
                                insertSkillsCommand.Parameters["@UpdatedDate"].Value = DateTime.Today;
                                insertSkillsCommand.Parameters["@SkillId"].Value =
                                    _skillValues.FirstOrDefault(x => x.Value == candidateSkill).Key;
                                insertSkillsCommand.ExecuteNonQuery();
                            }
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
            
            try
            {
                
                using (var connection = new SqlConnection(ConnectionString))
                {
                    
                    connection.Open();
                    using (SqlCommand selectCommand = new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetSelectAllQuery),connection ))

                    {
                        using (SqlDataReader datareader = selectCommand.ExecuteReader())
                        {
                            while (datareader.Read())
                            {
                                var candidate = HelperClass.GetCandidate(datareader);
                                candidates.Add(candidate);
                            }
                        }
                    }

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
            
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    
                    using (SqlCommand cmdGetCandidateById =
                        new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetSelectByIdQuery), connection))
                    {
                        cmdGetCandidateById.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader dataReader = cmdGetCandidateById.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                candidate = HelperClass.GetCandidate(dataReader);
                            }
                        }

                    }
                    List<Skill> candidateSkills = this.GetCandidateSkills(candidate.CandidateId);
                    SetCandidateSkills(candidateSkills, candidate);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return candidate;
        }
        
        /// <summary>
        /// Gets the candidate skills.
        /// </summary>
        /// <returns>A list of skills for all candidates</returns>
        public List<Skill> GetCandidateSkills()
        {
            List<Skill> candidateSkills = new List<Skill>();
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                  using ( SqlCommand cmdGetSkills = new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetSkillsQuery), connection))
                  {
                      using (SqlDataReader dataReader = cmdGetSkills.ExecuteReader())
                      {
                          while (dataReader.Read())
                          {
                              var skill = HelperClass.GetSkills(dataReader);
                              candidateSkills.Add(skill);
                          }
                      }
                  }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return candidateSkills;
        }

        /// <summary>
        /// Gets the candidate skills for a given candidate.
        /// </summary>
        /// <param name="candidateId">The candidate identifier.</param>
        /// <returns>A list of skills for a given candidate </returns>
        public List<Skill> GetCandidateSkills(string candidateId)
        {
            List<Skill> candidateSkills = new List<Skill>();
            
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmdGetSkills =
                        new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetSkillsByIdQuery), connection))
                    {
                        cmdGetSkills.Parameters.AddWithValue("@Id", candidateId);

                        using (SqlDataReader dataReader = cmdGetSkills.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var skill = HelperClass.GetSkills(dataReader);
                                candidateSkills.Add(skill);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
    }
}