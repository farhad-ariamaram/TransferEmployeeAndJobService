﻿using System;
using System.Collections.Generic;

#nullable disable

namespace TransferEmployeeAndJobService.ReqModels
{
    public partial class Log
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Response { get; set; }
        public bool Flag { get; set; }
    }
}
