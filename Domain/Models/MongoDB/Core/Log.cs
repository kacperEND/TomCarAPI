﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MongoDB
{
    public class Log : CoreAuditableMongoModel
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public LogType Type { get; set; }
        public string User { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class LogType
    {
        private LogType(string value) { Value = value; }

        public string Value { get; private set; }

        public static LogType Trace { get { return new LogType("Trace"); } }
        public static LogType Debug { get { return new LogType("Debug"); } }
        public static LogType Info { get { return new LogType("Info"); } }
        public static LogType Warning { get { return new LogType("Warning"); } }
        public static LogType Error { get { return new LogType("Error"); } }
    }
}