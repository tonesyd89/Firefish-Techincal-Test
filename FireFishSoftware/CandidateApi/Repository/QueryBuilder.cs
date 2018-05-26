using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace CandidateApi.Repository
{
    public class QueryBuilder
    {       
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
                    return this.getSkillsListQuery();
                default:
                    return string.Empty;
            }
        }

        private string getSkillsListQuery()
        {
            return "SELECT * from [dbo].[Skill]";
        }

        public string GetSqlCommand(Table table)
        {
            return this.GetMaxTableId(table);
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
    }
}