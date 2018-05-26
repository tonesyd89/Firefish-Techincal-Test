using System.Collections.Generic;
using CandidateApi.Models;

namespace CandidateApi.Repository
{
    public interface ICandidateRepository
    {
        /// <summary>
        /// Inserts the candidateto database.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns></returns>
        bool InsertCandidateToDatabase(Candidate candidate);

        IEnumerable<Candidate> GetAllCandidates();
        /// <summary>
        /// Gets the candidate skills.
        /// </summary>
        /// <param name="candidates">The candidates.</param>
        List<Skill> GetCandidateSkills();

        /// <summary>
        /// Gets the candidate by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Candidate GetCandidateById(int id);

        /// <summary>
        /// Gets the candidate skills.
        /// </summary>
        /// <param name="candidateID">The candidate identifier.</param>
        /// <returns></returns>
        List<Skill> GetCandidateSkills(string candidateID);

    }
}