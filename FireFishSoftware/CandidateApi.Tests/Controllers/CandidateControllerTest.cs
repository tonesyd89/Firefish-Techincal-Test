using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using CandidateApi.Controllers;
using CandidateApi.Models;
using CandidateApi.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CandidateApi.Tests.Controllers
{
    [TestClass]
    public class CandidateControllerTest
    {
        [TestMethod]
        public void GetWithIdTest()
        {
            //Arrange
            Mock<ICandidateRepository> mockCandidateRepository = new Mock<ICandidateRepository>();
            mockCandidateRepository.Setup(x => x.GetCandidateById(12)).Returns(new Candidate
                {
                    CandidateId = "12",
                    FirstName = "bob"
                });
            CandidateController controller = new CandidateController(mockCandidateRepository.Object);

            //Act
            IHttpActionResult actionResult = controller.Get(12);
            var contentResult = actionResult as OkNegotiatedContentResult<Candidate>;

            //Assert
            mockCandidateRepository.Verify(x => x.GetCandidateById(12), Times.Exactly(1));
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsNotNull(contentResult.Content.FirstName);
            Assert.AreEqual(contentResult.Content.CandidateId, "12");
        }

        [TestMethod]
        public void GetWithIdWrongIdTest()
        {
            //Arrange
            Mock<ICandidateRepository> mockCandidateRepository = new Mock<ICandidateRepository>();
            CandidateController controller = new CandidateController(mockCandidateRepository.Object);

            //Act
            IHttpActionResult actionResult = controller.Get(100);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTest()
        {
            //Arrange
            Mock<ICandidateRepository> mockCandidateRepository = new Mock<ICandidateRepository>();
            mockCandidateRepository.Setup(x => x.GetAllCandidates()).Returns(CreateCandidates());
            CandidateController controller = new CandidateController(mockCandidateRepository.Object);

            //Act
            IHttpActionResult actionResult = controller.Get();
            var contentResult= actionResult as OkNegotiatedContentResult<List<Candidate>>;
            List<Candidate> candidates = contentResult.Content ?? throw new ArgumentNullException("contentResult.Content");

            //Assert
            Assert.IsNotNull(candidates);
            Assert.AreEqual(candidates.Count, 9);
            int id = 1;
            for (int i = 0; i < candidates.Count; i++)
            {
                Assert.AreEqual(candidates[i].CandidateId, id.ToString());
                Assert.AreEqual(candidates[i].FirstName, $"FirstName{id.ToString()}");
                Assert.AreEqual(candidates[i].Surname, $"Surname{id.ToString()}");
                id++;
            }
        }

        [TestMethod]
        public void PostTest()
        {
            //Arrange
            Candidate updateCandidate = new Candidate
            {
                CandidateId = "123",
                FirstName = "bob"
            };

            Mock<ICandidateRepository> mockCandidateRepository = new Mock<ICandidateRepository>();
            mockCandidateRepository.Setup(x => x.InsertCandidateToDatabase(updateCandidate)).Returns(true);

            CandidateController controller = new CandidateController(mockCandidateRepository.Object);

            //Act
            IHttpActionResult response = controller.Post(updateCandidate);
            var contentResult = response as CreatedAtRouteNegotiatedContentResult<Candidate>;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual("DefaultApi", contentResult.RouteName);
            Assert.AreEqual("123", contentResult.RouteValues["id"]);
        }

        [TestMethod]
        public void PostMethodInternalServerError()
        {
            //Arrange
            Candidate updateCandidate = new Candidate
            {
                CandidateId = "123",
                FirstName = "bob"
            };

            Mock<ICandidateRepository> mockCandidateRepository = new Mock<ICandidateRepository>();
            mockCandidateRepository.Setup(x => x.InsertCandidateToDatabase(updateCandidate)).Returns(false);
            CandidateController controller = new CandidateController(mockCandidateRepository.Object);

            //Act
            IHttpActionResult response = controller.Post(updateCandidate);

            Assert.IsInstanceOfType(response, typeof(InternalServerErrorResult));

        }

        [TestMethod]
        public void PutReturnsContentResult()
        {
            // Arrange
            Candidate candidate = new Candidate
            {
                CandidateId = "123",
                FirstName = "bob"
            };

            var mockRepository = new Mock<ICandidateRepository>();
            var controller = new CandidateController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Put(candidate);
            var contentResult = actionResult as NegotiatedContentResult<Candidate>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual("123", contentResult.Content.CandidateId);
        }


        private List<Candidate> CreateCandidates()
        {
            List<Candidate> candidates = new List<Candidate>();
            int i = 1;
            while (i <10)
            {
                Candidate candidate = new Candidate
                    
                    {
                        CandidateId = i.ToString(),
                        FirstName =  $"FirstName{i}",
                        Surname = $"Surname{i}"

                    };
                candidates.Add(candidate);
                i++;
            }
            return candidates;

        }
    }
}
