using CrystalBlue.Validation.StandardRules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalBlue.Validation
{
    /// <summary>
    /// Class for handling the validation rules
    /// </summary>
    public class ValidationRuleHandler
    {
        /// <summary>
        /// Instantiates an instance of the Validation Rule Handler
        /// </summary>
        public ValidationRuleHandler( MainWindow window )
        {
            this.window = window;
        }

        /// <summary>
        /// Returns each standard rule
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<IValidationRule> GetStandardRules()
        {
            ObservableCollection<IValidationRule> rules = new ObservableCollection<IValidationRule>();

            rules.Add( new QuotationsRule( window ) );

            return rules;
        }

        private MainWindow window;
    }
}
