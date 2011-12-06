﻿using System.Runtime.Serialization;

namespace Epi.Web.WCF.SurveyService.Criteria
{
    /// <summary>
    /// Holds criteria for customer queries.
    /// </summary>
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SurveyInfoCriteria : Criteria
    {
        /// <summary>
        /// Unique customer identifier.
        /// </summary>
        [DataMember]
        public string SurveyId { get; set; }

        /// <summary>
        /// Flag as to whether to include order statistics.
        /// </summary>
        [DataMember]
        public bool IncludeOrderStatistics { get; set; }
    }
}
