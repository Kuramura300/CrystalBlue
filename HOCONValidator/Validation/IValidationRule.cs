using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CrystalBlue.HOCONValidator.Validation
{
    /// <summary>
    /// Interface used for defining a validation rule
    /// </summary>
    public interface IValidationRule
    {
        /// <summary>
        /// Whether the rule is enabled or not
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Name of the rule to be displayed on the Validation Rules list
        /// </summary>
        string RuleName { get; }

        /// <summary>
        /// Detailed description of the rule
        /// </summary>
        string RuleDescription { get; }

        /// <summary>
        /// Runs the validation logic for this rule
        /// </summary>
        void Run( string text );

        /// <summary>
        /// Displays the info for this rule
        /// </summary>
        void ShowInfo();
    }
}
