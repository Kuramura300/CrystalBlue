using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalBlue.Messages.ViewModels
{
    /// <summary>
    /// Holds information for a message that will be displayed in the DataGrid
    /// </summary>
    public class MessageViewModel
    {
        /// <summary>
        /// Creates a new message with given parameters to be displayed in the DataGrid
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lineNo"></param>
        /// <param name="line"></param>
        public MessageViewModel( string rule, string message, string lineNo, string line = null )
        {
            this.Rule = rule;
            this.Message = message;
            this.LineNo = lineNo;
            this.Line = line;
        }

        /// <summary>
        /// The rule this message comes from
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// The message to be displayed
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The line number this message is regarding (N/A for no line in particular)
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// The text from this line, if this message is regarding a line
        /// </summary>
        public string Line { get; set; }
    }
}
