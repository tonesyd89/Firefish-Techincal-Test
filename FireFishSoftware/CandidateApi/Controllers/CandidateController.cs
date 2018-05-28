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
        public IHttpActionResult Get()
        {
            List<Candidate> candidates = new List<Candidate>();
            try
            {
                candidates = this.candidateRepository.GetAllCandidates().ToList();
            }
            catch (Exception e)
            {
                NotFound();
            }

            return Ok(candidates);
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            Candidate candidate = this.candidateRepository.GetCandidateById(id);
            if (candidate == null)
            {
                return NotFound();
            }
            return Ok(candidate);
        }

        // POST api/values
        public IHttpActionResult Post([FromBody] Candidate candidate)
        {
            bool addedTodatabase = false;

            addedTodatabase = this.candidateRepository.InsertCandidateToDatabase(candidate);
            if (!addedTodatabase)
            {
                return InternalServerError();
            }

            return CreatedAtRoute("DefaultApi", new {id = candidate.CandidateId}, candidate);
        }

        // PUT api/values/5
        public IHttpActionResult Put([FromBody] Candidate candidate)
        {
            try
            {
                this.candidateRepository.UpdateCandidate(candidate);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
            return Content(HttpStatusCode.Accepted, candidate);
        }
    }
}
