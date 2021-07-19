using System;
using System.Collections.Generic;

#nullable disable

namespace TransferEmployeeAndJobService.ReqModels
{
    public partial class PayEducation
    {
        public PayEducation()
        {
            TblCustomerDegrees = new HashSet<TblCustomerDegree>();
        }

        public int EducationId { get; set; }
        public string EducationName { get; set; }

        public virtual ICollection<TblCustomerDegree> TblCustomerDegrees { get; set; }
    }
}
