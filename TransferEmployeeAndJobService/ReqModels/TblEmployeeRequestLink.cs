using System;
using System.Collections.Generic;

#nullable disable

namespace TransferEmployeeAndJobService.ReqModels
{
    public partial class TblEmployeeRequestLink
    {
        public string FldEmployeeRequestLinkId { get; set; }
        public DateTime? FldEmployeeRequestLinkExpireDate { get; set; }
        public string FldEmployeeRequestLinkPhone { get; set; }
    }
}
