using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandidateApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CandidateApi.Repository;

namespace CandidateApi.Tests.QueryBuilder
{
    [TestClass]
    public class TestQueryBuilder
    {
        private Repository.QueryBuilder queryBuilder;

        [TestInitialize]
        public void SetupTest()
        {
            this.queryBuilder = new Repository.QueryBuilder();
        }

        [TestMethod]
        public void  GetSqlCommandQueryTypeTest()
        {
            //arrange
            string expectedResult = "SELECT createdDate FROM Candidate WHERE id = @id";

            //act
            string actualResult =  queryBuilder.GetSqlCommand(QueryType.GetCreatedDateQuery);

            //assert
            Assert.IsTrue(expectedResult == actualResult);
        }

        [TestMethod]
        public void GetSqlCommandTableTest()
        {
            //arrange
            string expectedResult = "select MAX(Id) Id FROM Candidate";

            //act
            string actualResult = queryBuilder.GetSqlCommand(Table.Candidate);

            //assert
            Assert.IsTrue(expectedResult == actualResult);
        }

        [TestMethod]
        public void UpdateCandidateTest()
        {
            //arrange
            Candidate updateCandidate = this.CreateUpdateCandidate();
            string expectedOutput =
                "UPDATE Candidate SET FirstName = @FirstName,Surname = @Surname,DateOfBirth = @DateOfBirth,UpdatedDate = @UpdatedDate,CreatedDate = @CreatedDate WHERE Id = @id";

            //act
            string actualResult = queryBuilder.UpdateCandidate(updateCandidate);
        
            //assert
            Assert.IsTrue(actualResult == expectedOutput);

        }

        private Candidate CreateUpdateCandidate()
        {
            return new Candidate
            {
                CandidateId = "123",
                FirstName = "John",
                Surname = "Smith",
                DateOfBirth = DateTime.Parse("09-03-1989")
            };
        }


    }
}
