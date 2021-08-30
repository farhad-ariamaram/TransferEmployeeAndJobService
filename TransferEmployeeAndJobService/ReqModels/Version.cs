﻿using System;
using System.Collections.Generic;

#nullable disable

namespace TransferEmployeeAndJobService.ReqModels
{
    public partial class Version
    {
        public Version()
        {
            OutlineVersions = new HashSet<OutlineVersion>();
            Topics = new HashSet<Topic>();
            VersionWebsiteTables = new HashSet<VersionWebsiteTable>();
        }

        public long Id { get; set; }
        public string Version1 { get; set; }
        public string PersianVersion { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public string EnglishDescription { get; set; }
        public bool Active { get; set; }
        public int? SkillId { get; set; }

        public virtual ICollection<OutlineVersion> OutlineVersions { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<VersionWebsiteTable> VersionWebsiteTables { get; set; }
    }
}
