using System.Collections.Generic;
using System.Linq;
using CandidateApi.Models;
using CandidateApi.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CandidateApi.Tests.Repository
{
    [TestClass]
    public class TestCandidateRepository
    {
        private string ConnectionString;
    [TestMethod]
       public void  TestGetAllCandidates()
        {
            //Arrange
            CandidateRepository candidateRepository = new CandidateRepository();

            //Act
            List<Candidate> candidates = candidateRepository.GetAllCandidates().ToList();
            
            //Assert
            Assert.IsNotNull(candidates);
            Assert.AreEqual(100, candidates.Count);
        }
    }
}