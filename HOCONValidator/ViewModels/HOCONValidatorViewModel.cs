using CrystalBlue.Base;
using CrystalBlue.HOCONValidator.Validation;
using CrystalBlue.Messages.ViewModels;
using GalaSoft.MvvmLight.Command;
using Hocon;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CrystalBlue.HOCONValidator.ViewModels
{
    /// <summary>
    /// View Model for the class which holds the functionality for the HOCON Validator tool
    /// </summary>
    public class HOCONValidatorViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HOCONValidatorViewModel"/> class
        /// </summary>
        public HOCONValidatorViewModel()
        {
            // Create rule handler, and populate rules
            ruleHandler = new ValidationRuleHandler( this );
            PopulateRules();
        }

        //
        // Public Properties
        //

        /// <summary>
        /// Text for instruction, success or error message
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Colour for instruction, success or error message
        /// </summary>
        public Brush TextColour
        {
            get
            {
                return textColour;
            }
            set
            {
                textColour = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Shows the Reload File button when there is a file that has been opened
        /// </summary>
        public Visibility FileOpened
        {
            get
            {
                var visibility = Visibility.Collapsed;

                // If file is not null or empty, reload button will be visible
                if ( string.IsNullOrEmpty( filePath ) == false )
                {
                    visibility = Visibility.Visible;
                }

                return visibility;
            }
        }

        /// <summary>
        /// The name of the file to be displayed at the bottom of the view
        /// </summary>
        public string FileName
        {
            get
            {
                string value = "";

                if ( string.IsNullOrEmpty( filePath ) == false )
                {
                    value = "File: " + System.IO.Path.GetFileName( filePath );
                }

                return value;
            }
        }

        /// <summary>
        /// Collection of notifications/error messages to be displayed on the DataGrid
        /// </summary>
        public ObservableCollection<MessageViewModel> Messages { get; set; } = new ObservableCollection<MessageViewModel>();

        /// <summary>
        /// Collection of Standard Rules to be displayed on the view
        /// </summary>
        public ObservableCollection<IValidationRule> StandardRules { get; set; } = new ObservableCollection<IValidationRule>();

        //
        // Public Methods
        //

        /// <summary>
        /// Command that is run when the File Load button is clicked
        /// </summary>
        public ICommand FileLoadCommand => new RelayCommand( HandleFileLoadClick );

        /// <summary>
        /// Command that is run when the Reload File button is clicked
        /// </summary>
        public ICommand FileReloadCommand => new RelayCommand( HandleFileReloadClick );

        /// <summary>
        /// When Load File button is clicked, shows dialog for the user to choose a file to open, then attempts to parse as a HOCON file.
        /// If unable to parse, displays an error. Whether able to parse or not, it will check for validation errors based on enabled rules.
        /// </summary>
        public void HandleFileLoadClick()
        {
            Messages.Clear();

            OpenFileDialog dialog = new OpenFileDialog();

            if ( dialog.ShowDialog() == true )
            {
                filePath = dialog.FileName;

                fileText = System.IO.File.ReadAllText( filePath );

                OnPropertyChanged( "FileOpened" );
                OnPropertyChanged( "FileName" );

                try
                {
                    configFileData = HoconConfigurationFactory.ParseString( fileText );
                    Text = "No errors found! This is a valid .conf file.";
                    TextColour = Brushes.Green;
                }
                catch ( Exception ex )
                {
                    Text = ex.Message + "\n";
                    TextColour = Brushes.Red;
                }

                RunRules();
            }
        }

        /// <summary>
        /// When Reload File button is clicked, and if a file path is defined, attempts to parse as a HOCON file.
        /// If unable to parse, displays an error. Whether able to parse or not, it will check for validation errors based on enabled rules.
        /// </summary>
        public void HandleFileReloadClick()
        {
            Messages.Clear();

            if ( string.IsNullOrEmpty( filePath ) == false )
            {
                try
                {
                    configFileData = null;

                    fileText = System.IO.File.ReadAllText( filePath );

                    configFileData = HoconConfigurationFactory.ParseString( fileText );
                    Text = "No errors found! This is a valid .conf file.";
                    TextColour = Brushes.Green;
                }
                catch ( Exception ex )
                {
                    Text = ex.Message + "\n";
                    TextColour = Brushes.Red;
                }

                RunRules();
            }
        }

        //
        // Private Methods
        //

        /// <summary>
        /// Populates rule collections
        /// </summary>
        private void PopulateRules()
        {
            StandardRules = ruleHandler.GetStandardRules();
            OnPropertyChanged( "StandardRules" );
        }

        /// <summary>
        /// Run all enabled rules
        /// </summary>
        private void RunRules()
        {
            // Standard Rules
            foreach ( var rule in StandardRules.Where( x => x.IsEnabled == true ) )
            {
                rule.Run( fileText );
            }
        }

        //
        // Private Members
        //

        private string text = "Open a file to validate.";
        private Brush textColour = Brushes.Black;

        private string filePath = null;
        private string fileText = null;

        private Config configFileData = null;

        private ValidationRuleHandler ruleHandler = null;
    }
}
