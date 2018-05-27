using System.Collections.Generic;
using CandidateApi.Models;

namespace CandidateApi.Repository
{
    public interface ICandidateRepository
    {
        /// <summary>
        /// Inserts the updateCandidate to updateCandidate table in the database database.
        /// </summary>
        /// <param name="candidate">The updateCandidate.</param>
        /// <returns>true if updateCandidate added successfully
        ///  false if updateCandidate failed to add </returns>
        void InsertCandidateToDatabase(Candidate candidate);

        /// <summary>
        /// Gets all candidates.
        /// </summary>
        /// <returns>a collection of all candidates from the database</returns>
        IEnumerable<Candidate> GetAllCandidates();

        /// <summary>
        /// Gets the updateCandidate by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>an individual client based on their id </returns>
        Candidate GetCandidateById(int id);

        /// <summary>
        /// Updates the candidate and candidate skills.
        /// </summary>
        /// <param name="updateCandidate">The updateCandidate.</param>
        /// <returns>True if candidate updated successfully
        /// false if candidate not updated</returns>
        bool UpdateCandidate(Candidate updateCandidate);

        /// <summary>
        /// Inserts the into candidate skill.
        /// </summary>
        /// <param name="skillId">The skill identifier.</param>
        /// <param name="candidateId">The candidate identifier.</param>
        /// <param name="skillName">Name of the skill.</param>
        /// <param name="table">The table.</param>
        void InsertIntoCandidateSkill(int skillId, string candidateId, string skillName, Table table);

        /// <summary>
        /// Gets any  candidate skill from the input candidate which don't exist in the candidate skill table for a given candidate. 
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <param name="candidateId">The candidate identifier.</param>
        /// <returns></returns>
        IEnumerable<Skill> GetNewCandidateSkills(Candidate candidate, string candidateId);

        /// <summary>
        /// Gets the updateCandidate skills.
        /// </summary>
        /// <returns>A list of skills for all candidates</returns>
        List<Skill> GetCandidateSkills();

        /// <summary>
        /// Gets the updateCandidate skills for a given updateCandidate.
        /// </summary>
        /// <param name="candidateId">The updateCandidate identifier.</param>
        /// <returns>A list of skills for a given updateCandidate </returns>
        List<Skill> GetCandidateSkills(string candidateId);
    }
}