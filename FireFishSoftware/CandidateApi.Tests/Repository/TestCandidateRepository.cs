using System.Collections.Generic;
using System.Linq;
using CandidateApi.Controllers;
using CandidateApi.Models;
using CandidateApi.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CandidateApi.Tests.Repository
{
    [TestClass]
    public class TestCandidateRepository
    {
        private Mock<ICandidateRepository> _candidateRepository; 

        [TestInitialize]
        public void SetUpTests()
        {
            this._candidateRepository = new Mock<ICandidateRepository>();

        }

    [TestMethod]
       public void TestGetAllCandidates()
        { 
            //Arrange
            _candidateRepository.Setup(repository => repository.GetAllCandidates()).Returns(new List<Candidate>
            {
                new Candidate
                {
                    Address1 = "fake",
                    CandidateId = "1"
                }
            });
                
            CandidateController controller = new CandidateController( this._candidateRepository.Object);
            //Act
            
            var candidates = controller.Get().ToList();
            
            //Assert
            Assert.IsNotNull(candidates);
            Assert.AreEqual(110, candidates.Count);
        }

        [TestMethod]
        public void TestUpdateCandidates()
        {
            Candidate candidate = new Candidate()
            {
                FirstName = "Bob",
                Surname = "Jim",
                CandidateId = "101"
            };

            _candidateRepository.Setup(repository => repository.UpdateCandidate(candidate)).Returns(true);
            CandidateController controller = new CandidateController(this._candidateRepository.Object);
            
             controller.Post(candidate);
        }

    }
}