﻿using System;
using System.Collections.Generic;

#nullable disable

namespace TransferEmployeeAndJobService.ReqModels
{
    public partial class TblEmployeeRequestJobTitleFrom
    {
        public TblEmployeeRequestJobTitleFrom()
        {
            TblEmployeeRequestEmployeeRequests = new HashSet<TblEmployeeRequestEmployeeRequest>();
        }

        public int TblEmployeeRequestJobTitleFromId { get; set; }
        public string TblEmployeeRequestJobTitleFromTitle { get; set; }

        public virtual ICollection<TblEmployeeRequestEmployeeRequest> TblEmployeeRequestEmployeeRequests { get; set; }
    }
}
