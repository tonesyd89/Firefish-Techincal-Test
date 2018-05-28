using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CandidateApi.Models;
using CandidateApi.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CandidateApi.Tests.HelperClasses
{
    [TestClass]
    public class TestHelperClass
    {
    
        [TestMethod]
        public void TestGetCandidateParameters()
        {
            //Arrange

            SqlParameter[] expectedResults = CreateGetCandidateParametersExpectedReturn();
            Candidate candidate = CreateCandidate();

            SqlParameter[] actualResults = GetActualResultsCollection(HelperClass.GetCandidateUpdateParameters(candidate));
         
            //Assert
            Assert.IsTrue(AreArraysEqualString(actualResults, expectedResults));
        }

        [TestMethod]
        public void TestGetInsertSkillsParameter()
        {
            //Arrange
            SqlParameter[] expectedParameters = CreateGetInsertSkillsParametersExpectedReturn();

            //Act
            SqlParameter[] actualParameters = HelperClass.GetInsertSkillsParameters();

            //Assert
            Assert.IsTrue(AreArraysEqualSqlDbTypes(actualParameters, expectedParameters));
        }

        [TestMethod]
        public void TestGetCandidateupdateParameters()
        {
            //Arrange
            Candidate updateCandidate = this.CreateUpdateCandidate();
            SqlParameter[] expectedResults = CreateGetCandidateUpdateParametersParametersExpectedReturn();

            //Act
            SqlParameter[] actualResults = GetActualResultsCollection(HelperClass.GetCandidateUpdateParameters(updateCandidate));
           
            //Assert
            Assert.IsTrue(AreArraysEqualString(actualResults, expectedResults));
        }

        private static  bool AreArraysEqualString(SqlParameter[] actualResults, SqlParameter[] expectedResults)
        {
            if (actualResults.Length == expectedResults.Length)
            {
                foreach (var expectedResult in expectedResults)
                {
                    foreach (var actualResult in actualResults)
                    {
                        if (expectedResult.ParameterName == actualResult.ParameterName)
                        {
                            if (expectedResult.Value.ToString() != actualResult.Value.ToString())
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private static SqlParameter[] GetActualResultsCollection(SqlParameter[] actualResults)
        {
            SqlParameter updateddate = actualResults.FirstOrDefault(x => x.ParameterName == "@UpdatedDate");
            List<SqlParameter> intermediateResults = actualResults.ToList();
            intermediateResults.Remove(updateddate);
            actualResults = intermediateResults.ToArray();
            return actualResults;
        }

        private static  bool AreArraysEqualSqlDbTypes(SqlParameter[] actualResults, SqlParameter[] expectedResults)
        {
            if (actualResults.Length == expectedResults.Length)
            {
                foreach (var expectedResult in expectedResults)
                {
                    foreach (var actualResult in actualResults)
                    {
                        if (expectedResult.ParameterName == actualResult.ParameterName)
                        {
                            if (expectedResult.DbType!=  actualResult.DbType)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private static SqlParameter[] CreateGetInsertSkillsParametersExpectedReturn()
        {
            return new SqlParameter[]
            {
                new SqlParameter("@Id", SqlDbType.VarChar),
                new SqlParameter("@CandidateId", SqlDbType.VarChar),
                new SqlParameter("@CreatedDate", SqlDbType.DateTime),
                new SqlParameter("@UpdatedDate", SqlDbType.DateTime),
                new SqlParameter("@SkillId", SqlDbType.VarChar)
            };
        }

        private static SqlParameter[] CreateGetCandidateParametersExpectedReturn()
        {
            return  new SqlParameter[]
            {
                new SqlParameter("@Id", "123"),
                new SqlParameter("@FirstName", "John"),
                new SqlParameter("@Surname", "Smith"),
                new SqlParameter("@DateOfBirth", DateTime.Parse("09-03-1989")),
                new SqlParameter("Address1", "123 Fake Street"),
                new SqlParameter("@Town", "ATown"),
                new SqlParameter("@Country", "Scotland"),
                new SqlParameter("@Postcode", "G1 1PO"),
                new SqlParameter("@PhoneHome", "0141123456"),
                new SqlParameter("@PhoneMobile", "077456789"),
                new SqlParameter("@PhoneWork", "0131312456")
            };
        }

        private static SqlParameter[] CreateGetCandidateUpdateParametersParametersExpectedReturn()
        {
            return new SqlParameter[]
            {
                new SqlParameter("@Id", "123"),
                new SqlParameter("@FirstName", "John"),
                new SqlParameter("@Surname", "Smith"),
                new SqlParameter("@DateOfBirth",  DateTime.Parse("09-03-1989"))
            };

        }

        private Candidate CreateCandidate()
        {
            return new Candidate
                    {
                        CandidateId = "123",
                        Skills = new List<string>{ "Microsoft Word", "C#"},
                        FirstName = "John",
                        Surname = "Smith",
                        DateOfBirth = DateTime.Parse("09-03-1989"),
                        Address1 =  "123 Fake Street",
                        Town =  "ATown",
                        Country = "Scotland",
                        Postcode = "G1 1PO",
                        PhoneHome = "0141123456",
                        PhoneMobile = "077456789",
                        PhoneWork = "0131312456"                        
                    };
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
