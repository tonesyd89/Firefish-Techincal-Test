using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CandidateApi.Repository
{
    public class Skill
    {
        /// <summary>
        /// Gets or sets the skill.
        /// </summary>
        /// <value>
        /// The skill.
        /// </value>
        public string skill { get; set; }

        /// <summary>
        /// Gets or sets the candidate identifier.
        /// </summary>
        /// <value>
        /// The candidate identifier.
        /// </value>
        public string CandidateId { get; set; }

        /// <summary>
        /// Gets or sets the skill identifier.
        /// </summary>
        /// <value>
        /// The skill identifier.
        /// </value>
        public string SkillId { get; set; }
    }
}