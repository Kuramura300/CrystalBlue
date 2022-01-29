using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;
using Hocon;
using Microsoft.Win32;
using CrystalBlue.Messages.ViewModels;
using CrystalBlue.Validation;

namespace CrystalBlue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

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
        /// Width of the window used for scroll viewer auto showing scrollbars
        /// </summary>
        public double ScreenWidth
        {
            get
            {
                double value = this.ActualWidth;
                OnPropertyChanged( "GeneralTextBoxWidth" );
                return value;
            }
        }

        /// <summary>
        /// Height of the window used for scroll viewer auto showing scrollbars
        /// </summary>
        public double ScreenHeight
        {
            get
            {
                return this.ActualHeight;
            }
        }

        /// <summary>
        /// Max width used for the text for the instruction, success or error message
        /// </summary>
        public double GeneralTextBoxWidth
        {
            get
            {
                double value = this.ActualWidth - 200;

                if (value <= 0)
                {
                    value = 1;
                }

                return value;
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
                if (string.IsNullOrEmpty(filePath) == false)
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

                if (string.IsNullOrEmpty(filePath) == false)
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

        //public void Cursed(IHoconElement value)
        //{
        //    var children = value.Children;

        //    foreach ( var child in children )
        //    {
        //        foreach ( var field in child.GetObject() )
        //        {
        //            Key k = new Key();
        //            k.KeyName = field.Key;
        //        }

        //        //Key k = new Key();
        //        //k.KeyName = key.GetString();
        //        //k.children = key.GetArray();
        //        //k.elements = key.Children;

        //        //foreach ( var item in k.children )
        //        //{
        //        //    Cursed( item );
        //        //}
        //    }
        //}

        //
        // Public Methods
        //

        /// <summary>
        /// When Load File button is clicked, shows dialog for the user to choose a file to open, then attempts to parse as a HOCON file.
        /// If unable to parse, displays an error. Whether able to parse or not, it will check for validation errors based on enabled rules.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleFileLoadClick( object sender, RoutedEventArgs e )
        {
            Messages.Clear();

            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
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

                    //if ( ex.Message.Contains( "Error while tokenizing Hocon") )
                    //{
                    //    MessageViewModel messageVM = new MessageViewModel( "Check your brackets.", "N/A" );
                    //    Messages.Add( messageVM );
                    //    OnPropertyChanged( "Messages" );
                    //}
                }

                RunRules();
            }
        }

        /// <summary>
        /// When Reload File button is clicked, and if a file path is defined, attempts to parse as a HOCON file.
        /// If unable to parse, displays an error. Whether able to parse or not, it will check for validation errors based on enabled rules.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleFileReloadClick( object sender, RoutedEventArgs e )
        {
            Messages.Clear();

            if (string.IsNullOrEmpty(filePath) == false)
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

                    //if ( ex.Message.Contains( "Error while tokenizing Hocon" ) )
                    //{
                    //    MessageViewModel messageVM = new MessageViewModel( "Check your brackets.", "N/A" );
                    //    Messages.Add( messageVM );
                    //    OnPropertyChanged( "Messages" );
                    //}
                }

                RunRules();
            }
        }

        /// <summary>
        /// When the window is resized, raise property changed for screen width/height for the scrollbars to update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleScreenResized( object sender, RoutedEventArgs e )
        {
            OnPropertyChanged( "ScreenWidth" );
            OnPropertyChanged( "ScreenHeight" );
        }

        //
        // Property Changed Event
        //

        /// <summary>
        /// Property Changed Event Handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handles the raising of property changed to update the view
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged( [CallerMemberName] string name = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
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

        private Config configFileData;

        private ValidationRuleHandler ruleHandler = null;
    }
}
