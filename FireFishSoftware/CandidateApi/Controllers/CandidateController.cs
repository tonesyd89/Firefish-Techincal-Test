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

        public CandidateController():this(new CandidateRepository())
        {
            
        }

        public CandidateController(ICandidateRepository candidateRepository)
        {
            this.candidateRepository = candidateRepository;
        }

        // GET api/values
        public IEnumerable<Candidate> Get()
        {
            List<Candidate> candidates = this.candidateRepository.GetAllCandidates().ToList();
            return candidates;
            //return new string[] { "value1", "value2" };

        }
        
        // GET api/values/5
        public Candidate Get(int id)
        {
            Candidate candidate = this.candidateRepository.GetCandidateById(id);
            return candidate;
        }

        // POST api/values
        public void Post([FromBody]Candidate candidate)
        {
            this.candidateRepository.InsertCandidateToDatabase(candidate);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
