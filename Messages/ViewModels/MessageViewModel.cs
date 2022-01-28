using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalBlue.Messages.ViewModels
{
    public class MessageViewModel
    {
        public MessageViewModel( string message, string lineNo, string line = null )
        {
            this.Message = message;
            this.LineNo = lineNo;
            this.Line = line;
        }

        public string Message { get; set; }

        public string LineNo { get; set; }

        public string Line { get; set; }
    }
}
