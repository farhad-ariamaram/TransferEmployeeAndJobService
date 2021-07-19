using System;
using System.Collections.Generic;

#nullable disable

namespace TransferEmployeeAndJobService.Models
{
    public partial class TblLink
    {
        public string Id { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Phone { get; set; }
    }
}
