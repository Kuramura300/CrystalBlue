using CrystalBlue.Messages.ViewModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CrystalBlue.Validation.StandardRules
{
    /// <summary>
    /// Rule regarding quotations
    /// </summary>
    class QuotationsRule : IValidationRule
    {
        /// <summary>
        /// Instantiates a new instance of this rule
        /// </summary>
        /// <param name="window"></param>
        public QuotationsRule( MainWindow window )
        {
            this.window = window;
        }

        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc />
        public string RuleName { get; } = "Quotations";

        /// <inheritdoc />
        public string RuleDescription { get; } = "Checks for any lines which only have one quotation mark, and suggests that there may be a missing quotation.";

        /// <inheritdoc />
        public void Run( string text )
        {
            // Splits text into an array separated by new line
            var lines = text.Split( '\n' );

            // Iterate through each line
            for ( int i = 0; i < lines.Length; i++ )
            {
                // Gets count of quotations in this line
                var quotations = lines[i].Count( x => x == '\"' );

                // Check if there is more than 1 quotation, but less than 2
                if ( quotations > 0 && quotations < 2 )
                {
                    // Get real line number and set error message
                    var lineNo = i + 1;
                    var message = ( "Potentially missing quotation?" );

                    // Create message to be displayed on the view and add it
                    MessageViewModel messageVM = new MessageViewModel( RuleName, message, lineNo.ToString(), lines[i].Trim() );
                    window.Messages.Add( messageVM );
                    window.OnPropertyChanged( "Messages" );
                }
            }
        }

        /// <summary>
        /// Runs ShowInfo when button clicked
        /// </summary>
        public ICommand ShowInfoCommand => new RelayCommand( ShowInfo );

        /// <inheritdoc />
        public void ShowInfo()
        {
            MessageBox.Show( RuleDescription, RuleName );
        }

        private MainWindow window = null;
    }
}
