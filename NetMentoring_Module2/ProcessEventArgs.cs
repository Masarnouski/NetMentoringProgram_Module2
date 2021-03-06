﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMentoring_Module2
{
    public class ProcessEventArgs
    {
        public string Message{ get; }

        public int Count { get; }

        public string Directory { get; }

        public bool IsCancelled { get; set; }

        public bool IsExcluded { get; set; }

        public ProcessEventArgs(string message, int count, string directory, bool isExcluded, bool isCancelled)
        {
            Directory = directory;
            Message = message;
            Count = count;
            IsCancelled = isCancelled;
            IsExcluded = isExcluded;
        }
    }
}
