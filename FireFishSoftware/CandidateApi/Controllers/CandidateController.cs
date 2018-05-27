using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using CandidateApi.Models;
using CandidateApi.Repository;

namespace CandidateApi.Controllers
{
    public class CandidateController : ApiController
    {
        private ICandidateRepository candidateRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandidateController"/> class.
        /// </summary>
        public CandidateController():this(new CandidateRepository())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CandidateController"/> class.
        /// </summary>
        /// <param name="candidateRepository">The candidate repository.</param>
        public CandidateController(ICandidateRepository candidateRepository)
        {
            this.candidateRepository = candidateRepository;
        }

        // GET api/values
        public IEnumerable<Candidate> Get()
    {
            List<Candidate> candidates = new List<Candidate>();
            try
            {
                candidates = this.candidateRepository.GetAllCandidates().ToList();
            }
            catch (Exception e)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Get response failed. Message: {e.Message}'"),
                    ReasonPhrase = ""
                };
                throw new HttpResponseException(resp);
            }

            return candidates;
        }

        // GET api/values/5
        public Candidate Get(int id)
        {
            Candidate candidate = this.candidateRepository.GetCandidateById(id);
            return candidate;
        }

        // POST api/values
        public void Post([FromBody] Candidate candidate)
        {
            try
            {
                this.candidateRepository.InsertCandidateToDatabase(candidate);
            }
            catch (Exception e)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Insert failed with message: {e.Message}'"),
                    ReasonPhrase = ""
                };
                throw new HttpResponseException(resp);
            }
        }

        // PUT api/values/5
        public void Put([FromBody] string id,  Candidate candidate)
        {
            try
            {
                this.candidateRepository.UpdateCandidate(id, candidate);
            }
            catch (Exception e)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Update candidate failed with message: {e.Message}'"),
                    ReasonPhrase = ""
                };
                throw new HttpResponseException(resp);
            }
        }
    }
}
