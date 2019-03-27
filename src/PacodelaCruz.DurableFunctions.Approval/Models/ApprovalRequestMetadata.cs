﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacodelaCruz.DurableFunctions.Models
{
    public class ApprovalRequestMetadata
    {
        public string ApprovalType { get; set; }
        public string InstanceId { get; set; }
        public string ReferenceUrl { get; set; }
        public string ApplicantId { get; set; }
        public string ApplicationName { get; set; }
    }
}
