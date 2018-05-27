using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CandidateApi.Repository
{
    public enum QueryType
    {
        /// <summary>
        /// The get update command
        /// </summary>
        GetInsertQuery,

        /// <summary>
        /// The get select all command
        /// </summary>
        GetSelectAllQuery,

        /// <summary>
        /// The get select by identifier command
        /// </summary>
        GetSelectByIdQuery,

        /// <summary>
        /// The get skills command
        /// </summary>
        GetSkillsQuery,

        /// <summary>
        /// The get skills by identifier query
        /// </summary>
        GetSkillsByIdQuery,

        /// <summary>
        /// The get maximum candidate identifier
        /// </summary>
        GetMaxIdCandidateQuery,

        /// <summary>
        /// The get maximum identifier candidate skill query
        /// </summary>
        GetMaxIdCandidateSkillQuery,

        /// <summary>
        /// The insert skills query
        /// </summary>
        GetInsertSkillsQuery,

        /// <summary>
        /// The get skills list
        /// </summary>
        GetSkillsListQuery,

        /// <summary>
        /// The get created date query
        /// </summary>
        GetCreatedDateQuery
    }
}