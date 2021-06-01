﻿using System;

namespace Domain.Common
{
    public abstract class AuditableModel : CoreAuditableModel
    {
        public int? CreatedBy { get; internal set; }
        public DateTime? CreatedOnUtc { get; internal set; }
        public int? ModifiedBy { get; internal set; }
        public DateTime? ModifiedOnUtc { get; internal set; }
    }
}