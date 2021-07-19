using System;
using System.Collections.Generic;

#nullable disable

namespace TransferEmployeeAndJobService.Models
{
    public partial class TblMilitaryOrganization
    {
        public TblMilitaryOrganization()
        {
            TblUserMilitaries = new HashSet<TblUserMilitary>();
        }

        public int Id { get; set; }
        public string OrganizationName { get; set; }

        public virtual ICollection<TblUserMilitary> TblUserMilitaries { get; set; }
    }
}
