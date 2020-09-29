using System;
using System.Collections.Generic;
using System.Text;

namespace CloudNinjaFunctions.Helpers
{
    public class ProjectStatus
    {
        public Guid proj_id { get; set; }
        public string prjrequest_rcvd { get; set; }
        public string initial_meeting { get; set; }
        public string get_requirements { get; set; }
        public string crt_visio { get; set; }
        public string crt_fwnw_doc { get; set; }
        public string crt_sizingconfig_doc { get; set; }
        public string updt_sdd { get; set; }
        public string send_rvwto_prjtm { get; set; }
        public string prjtm_rvw_status { get; set; }
        public string send_rvwto_tad { get; set; }
        public string tad_rvw_status { get; set; }
        public string send_rvwto_isrm { get; set; }
        public string isrm_rvw_status { get; set; }
        public string send_signoff_cspe { get; set; }
        public string cspe_signed { get; set; }
        public string send_signoff_appmgr { get; set; }
        public string appmgr_signed { get; set; }
        public string send_signoff_tad { get; set; }
        public string tad_signed { get; set; }
        public string send_signoff_isrm { get; set; }
        public string isrm_signed { get; set; }
        public string send_signoff_sdm { get; set; }
        public string sdm_signed { get; set; }
        public string crt_deploydoc { get; set; }
        public string deploydoc_approval { get; set; }
        public string send_appreg_approval { get; set; }
        public string appreg_approval_status { get; set; }
        public string appreg_status { get; set; }
        public string azres_deploy_status { get; set; }
        public string send_nwfw_approval { get; set; }
        public string nwfw_approval_status { get; set; }
        public string send_iam_approval { get; set; }
        public string iam_approval_status { get; set; }
        public string grant_roles_perms { get; set; }
        public string crt_handover_doc { get; set; }
        public string send_handover_doc { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }
        public string created_by { get; set; }
        public string modified_by { get; set; }
    }
}
