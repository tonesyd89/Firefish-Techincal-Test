using System;
using System.Text;
using CandidateApi.Models;

namespace CandidateApi.Repository
{
    public class QueryBuilder
    {
        /// <summary>
        /// Gets the SQL command.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public string GetSqlCommand(QueryType query)
        {
            switch (query)
            {
                case QueryType.GetSelectAllQuery:
                    return this.GetSelectAllCommand();
                case QueryType.GetSelectByIdQuery:
                    return this.GetSelectByIdQuery();
                case QueryType.GetInsertQuery:
                    return this.GetUpdateQuery();
                case QueryType.GetSkillsQuery:
                    return this.GetSkillsQuery();
                case QueryType.GetSkillsByIdQuery:
                    return this.GetSkillsByIdQuery();
                case QueryType.GetInsertSkillsQuery:
                    return this.GetInsertSkillsQuery();
                case QueryType.GetSkillsListQuery:
                    return this.GetSkillsListQuery();
                case QueryType.GetCreatedDateQuery:
                    return this.GetCreatedDatequery();
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets the SQL command.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public string GetSqlCommand(Table table)
        {
            return this.GetMaxTableId(table);
        }

        /// <summary>
        /// Gets the update candidate command.
        /// </summary>
        /// <param name="candidateUpdate">The candidate update.</param>
        /// <returns> a query string to update the candidate table</returns>
        public string UpdateCandidate(Candidate candidateUpdate)
        {
            StringBuilder sqlUpdateQuery = new StringBuilder();
      
            sqlUpdateQuery.Append("UPDATE Candidate ");
            sqlUpdateQuery.Append(BuildUpdateSetString(candidateUpdate));

            return sqlUpdateQuery.ToString();
        }

        private string GetSkillsListQuery()
        {
            return "SELECT * from [dbo].[Skill]";
        }

        private string GetInsertSkillsQuery()
        {
            return @"INSERT INTO [dbo].[CandidateSkill]
                    (
                        [ID],
                        [CandidateID],
                        [CreatedDate],
                        [UpdatedDate],
                        [SkillID]
                    )                 
                    VALUES
                    (   @Id,         
                        @CandidateID, 
                        @CreatedDate, 
                        @UpdatedDate, 
                        @SkillId      
                        )";
        }

        /// <summary>
        /// Gets the maximum candidate table identifier.
        /// </summary>
        /// <returns></returns>
        private string GetMaxTableId(Table table)
        {
            return "select MAX(Id) Id from " +  Enum.GetName(table.GetType(), table);
        }

        /// <summary>
        /// Gets the skills query.
        /// </summary>
        /// <returns></returns>
        private string GetSkillsQuery()
        {
            return @"SELECT [S].[Name], c.[ID] FROM [dbo].[Candidate] AS [C] JOIN [dbo].[CandidateSkill] AS [CS] ON [CS].[CandidateID] = [C].[ID]
                                 JOIN [dbo].[Skill] AS [S] ON [S].[ID] = [CS].[SkillID]";
        }
        /// <summary>
        /// Gets the skills by identifier query.
        /// </summary>
        /// <returns></returns>
        private string GetSkillsByIdQuery()
        {
            return @"SELECT [S].[Name], c.[ID] FROM [dbo].[Candidate] AS [C] JOIN [dbo].[CandidateSkill] AS [CS] ON [CS].[CandidateID] = [C].[ID]
                                 JOIN [dbo].[Skill] AS [S] ON [S].[ID] = [CS].[SkillID] WHERE C.id = @id";
        }

        /// <summary>
        /// Gets the update command.
        /// </summary>
        /// <returns></returns>
        private string GetUpdateQuery()
        {
            return 
                @" INSERT INTO[dbo].[Candidate]
               (
                 [Id]
                , [FirstName]
                ,[Surname]
                ,[DateOfBirth]
                ,[Address1]
                ,[Town]
                ,[Country]
                ,[PostCode]
                ,[PhoneHome]
                ,[PhoneMobile]
                ,[PhoneWork]
                ,[CreatedDate]
                ,[UpdatedDate]  
                )
            VALUES
               (
                @Id,
                @FirstName, 
                @Surname,
                @DateOfBirth,
                @Address1, 
                @Town, 
                @Country,
                @PostCode,
                @PhoneHome, 
                @PhoneMobile,
                @PhoneWork, 
                @CreatedDate,
                @UpdatedDate
               )";
        }

        /// <summary>
        /// Gets the select by identifier command.
        /// </summary>
        /// <returns></returns>
        private string GetSelectByIdQuery()
        {
            return "SELECT * from Candidate where id = @Id";

        }

        /// <summary>
        /// Gets the select all command.
        /// </summary>
        /// <returns></returns>
        private string GetSelectAllCommand()
        {
            return "SELECT * from Candidate";
        }

        private string GetCreatedDatequery()
        {
            return "SELECT createdDate FROM Candidate WHERE id = @id";
        }

        private static string BuildUpdateSetString(Candidate candidateUpdate)
        {
            StringBuilder setString = new StringBuilder("SET ");

           
                if (candidateUpdate.FirstName != null)
                {
                    setString.Append("FirstName = @FirstName" + ",");
                }

                if (candidateUpdate.Surname != null)
                {
                    setString.Append("Surname = @Surname"  + ",");
                }

                if (candidateUpdate.DateOfBirth != default(DateTime))
                {
                    setString.Append("DateOfBirth = @DateOfBirth" + ",");
                }

                if (candidateUpdate.Address1 != null)
                {
                    setString.Append("Address1 = @Address1" + ",");
                }

                if (candidateUpdate.Town != null)
                {
                    setString.Append("Town = @Town" + ",");
                }

                if (candidateUpdate.Country != null)
                {
                    setString.Append("Country = @Country" + ",");
                }

                if (candidateUpdate.Postcode != null)
                {
                    setString.Append("Postcode = @Postcode" + ",");
                }

                if (candidateUpdate.PhoneHome != null)
                {
                    setString.Append("PhoneHome = @PhoneHome" + ",");
                }

                if (candidateUpdate.PhoneMobile != null)
                {
                    setString.Append("PhoneMobile = @PhoneMobile" + ",");
                }

                if (candidateUpdate.PhoneWork != null)
                {
                    setString.Append("PhoneWork = @PhoneWork" + ",");
                }
           
            setString.Append("UpdatedDate = @UpdatedDate" +",");
            setString.Append("CreatedDate = @CreatedDate" + ",");

            setString.Append(" WHERE Id = @id");
            int indexOfLastComma = setString.ToString().LastIndexOf(',');
            return setString.ToString().Remove(indexOfLastComma, 1);
        }
    }
}