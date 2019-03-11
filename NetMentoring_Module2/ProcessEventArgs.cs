using System;
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
 
        public ProcessEventArgs(string message, int count, string directory)
        {
            Directory = directory;
            Message = message;
            Count = count;
        }
    }
}
