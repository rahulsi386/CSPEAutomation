using Microsoft.Recognizers.Text.DateTime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudNinja.Helpers
{
    public class ProjectInfo
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string ProjectOwnerEmail { get; set; }
        public string AzureServicesUsed { get; set; }
        public string ClarityId { get; set; }
        public string eHLCCD { get; set; }
        public string CostCenter { get; set; }
        public string BillingContactEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
