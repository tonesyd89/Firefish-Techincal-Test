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

        /// <summary>
        /// Gets the  current skills from the skills table in the database. This is used to add updateCandidate to the updateCandidate skills table.
        /// </summary>
        /// <returns>All the current skills alongside their id from the databse</returns>
        private Dictionary<string, string> GetSkills()
        {
            Dictionary<string, string> skillsDictionary = new Dictionary<string, string>();

            try
            {


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
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Inserts the Candidate to Candidate table in the database database.
        /// </summary>
        /// <param name="candidate">The updateCandidate.</param>
        /// <returns>true if Candidate added successfully
        ///  false if Candidate failed to add </returns>
        public bool InsertCandidateToDatabase(Candidate candidate)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    int candidateId = GetSkillId(Table.Candidate);
                    candidateId++;
                    candidate.CandidateId = candidateId.ToString();

                    using (SqlCommand insertCommand =
                        new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetInsertQuery), connection))
                    {
                        insertCommand.Parameters.AddRange(HelperClass.GetCandidateParameters(candidate));
                        insertCommand.ExecuteNonQuery();
                    }

                    InsertCandidateSkillToDb(candidate);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Inserts the Candidate skill to Candidate skill table in the database.
        /// </summary>
        /// <param name="candidate">The updateCandidate.</param>
        private void InsertCandidateSkillToDb(Candidate candidate)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    int skillId = GetSkillId(Table.CandidateSkill);

                    foreach (var candidateSkill in candidate.Skills)
                    {
                        skillId++;
                        this.InsertIntoCandidateSkill(skillId, candidate.CandidateId, candidateSkill,
                            Table.CandidateSkill);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets the skill identifier.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>the maximum id of the given table </returns>
        private int GetSkillId(Table table)
        {
            try
            {
                int skillId;
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand getMaxId =
                        new SqlCommand(this._queryBuilder.GetSqlCommand(table), connection))
                    {
                        skillId = (int) getMaxId.ExecuteScalar();
                    }
                }

                return skillId;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Gets all candidates.
        /// </summary>
        /// <returns>a collection of all candidates from the database</returns>
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
                throw e;
            }
            return candidates;
        }

        /// <summary>
        /// Gets the updateCandidate by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>an individual client based on their id </returns>
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
                throw e;
            }
            return candidate;
        }

        /// <summary>
        /// Updates the candidate and candidate skills.
        /// </summary>
        /// <param name="updateCandidate">The updateCandidate.</param>
        /// <returns>
        /// True if candidate updated successfully
        /// false if candidate not updated
        /// </returns>
        public bool UpdateCandidate( Candidate updateCandidate)
        {
            try
            {
                string candidateId = updateCandidate.CandidateId;
                using (var connection = new SqlConnection(ConnectionString))
                {
                    
                        var newCandidateSkills = this.GetNewCandidateSkills(updateCandidate, candidateId);
                        this.InsertCandidateSkillToDb(newCandidateSkills, candidateId);
                    
                    connection.Open();
                    using (SqlCommand cmdUpdatecandidateCommand =
                        new SqlCommand(this._queryBuilder.UpdateCandidate(updateCandidate), connection))
                    {
                        using (SqlCommand cmdgetCreatedDateCommand =
                            new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetCreatedDateQuery), connection))
                        {
                            cmdgetCreatedDateCommand.Parameters.AddWithValue("@Id", candidateId);
                            DateTime candidateCreatedDate = DateTime.Parse(cmdgetCreatedDateCommand.ExecuteScalar().ToString());

                            cmdUpdatecandidateCommand.Parameters.AddRange(
                                HelperClass.GetCandidateUpdateParameters(updateCandidate));

                            cmdUpdatecandidateCommand.Parameters.Add(new SqlParameter("@CreatedDate",
                                candidateCreatedDate));

                            cmdUpdatecandidateCommand.ExecuteNonQuery();
                            return true;
                        }

                    }
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Inserts the candidate skill to database.
        /// </summary>
        /// <param name="newCandidateSkills">The new candidate skills.</param>
        /// <param name="candidateId">The candidate identifier.</param>
        private void InsertCandidateSkillToDb(IEnumerable<Skill> newCandidateSkills,string candidateId)
        {
                    int skillId = GetSkillId(Table.CandidateSkill);
                    foreach (Skill newCandidateSkill in newCandidateSkills)
                    {
                        skillId++;
                        this.InsertIntoCandidateSkill(skillId, candidateId, newCandidateSkill.skill, Table.CandidateSkill);
                    }
        }

        /// <summary>
        /// Inserts the into candidate skill.
        /// </summary>
        /// <param name="skillId">The skill identifier.</param>
        /// <param name="candidateId">The candidate identifier.</param>
        /// <param name="skillName">Name of the skill.</param>
        /// <param name="table">The table.</param>
        public void InsertIntoCandidateSkill(int skillId, string candidateId, string skillName, Table table)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand insertSkillCommand =
                        new SqlCommand(this._queryBuilder.GetSqlCommand(QueryType.GetInsertSkillsQuery), connection))
                    {
                        insertSkillCommand.Parameters.AddRange(HelperClass.GetInsertSkillsParameters());
                        skillId++;
                        insertSkillCommand.Parameters["@Id"].Value = skillId;
                        insertSkillCommand.Parameters["@CandidateId"].Value = candidateId;
                        insertSkillCommand.Parameters["@CreatedDate"].Value = DateTime.Now;
                        insertSkillCommand.Parameters["@UpdatedDate"].Value = DateTime.Now;
                        insertSkillCommand.Parameters["@SkillId"].Value =
                            _skillValues.FirstOrDefault(x => x.Value == skillName).Key;
                        insertSkillCommand.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets any  candidate skill from the input candidate which don't exist in the candidate skill table for a given candidate. 
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <param name="candidateId">The candidate identifier.</param>
        /// <returns></returns>
        public IEnumerable<Skill> GetNewCandidateSkills(Candidate candidate, string candidateId)
        {
            List<Skill> existingCandidateSkills = this.GetCandidateSkills(candidate.CandidateId);
            List<Skill> updateCandidateSkills = new List<Skill>();

            foreach (string candidateSkill in candidate.Skills)
            {
                Skill skill = new Skill()
                {
                    CandidateId = candidateId,
                    skill = candidateSkill
                };
                updateCandidateSkills.Add(skill);
            }
            IEnumerable<Skill> result = updateCandidateSkills.Where(p => existingCandidateSkills.All(p2 => p2.skill != p.skill));
            return result;
        }

        /// <summary>
        /// Gets the updateCandidate skills.
        /// </summary>
        /// <returns>A list of skills for all candidates</returns>
        public List<Skill> GetCandidateSkills()
        {
            List<Skill> candidateSkills = new List<Skill>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
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
        /// Gets the updateCandidate skills for a given updateCandidate.
        /// </summary>
        /// <param name="candidateId">The updateCandidate identifier.</param>
        /// <returns>A list of skills for a given updateCandidate </returns>
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
                                Skill skill = HelperClass.GetSkills(dataReader);
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
        /// Sets the Candidate skills.
        /// </summary>
        /// <param name="candidateSkills">The candidates skills.</param>
        /// <param name="candidate">The updateCandidate.</param>
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