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

namespace CrystalBlue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

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

        public double ScreenWidth
        {
            get
            {
                double value = this.ActualWidth;
                OnPropertyChanged( "GeneralTextBoxWidth" );
                return value;
            }
        }

        public double ScreenHeight
        {
            get
            {
                return this.ActualHeight;
            }
        }

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

        public ObservableCollection<MessageViewModel> Messages { get; set; } = new ObservableCollection<MessageViewModel>();

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

        public void CheckForErrors()
        {
            var lines = fileText.Split( '\n' );

            for ( int i = 0; i < lines.Length; i++ )
            {
                var quotations = lines[i].Count( x => x == '\"' );

                if ( quotations > 0 && quotations < 2 )
                {
                    var lineNo = i + 1;
                    var message = ("Potentially missing quotation?");

                    MessageViewModel messageVM = new MessageViewModel( message, lineNo.ToString(), lines[i].Trim() );
                    Messages.Add( messageVM );
                    OnPropertyChanged( "Messages" );
                }
            }
        }
        public void HandleFileLoadClick(object sender, RoutedEventArgs e)
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

                    if ( ex.Message.Contains( "Error while tokenizing Hocon") )
                    {
                        MessageViewModel messageVM = new MessageViewModel( "Check your brackets.", "N/A" );
                        Messages.Add( messageVM );
                        OnPropertyChanged( "Messages" );
                    }
                }

                CheckForErrors();
            }
        }

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

                    if ( ex.Message.Contains( "Error while tokenizing Hocon" ) )
                    {
                        MessageViewModel messageVM = new MessageViewModel( "Check your brackets.", "N/A" );
                        Messages.Add( messageVM );
                        OnPropertyChanged( "Messages" );
                    }
                }

                CheckForErrors();
            }
        }

        public void HandleScreenResized(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged( "ScreenWidth" );
            OnPropertyChanged( "ScreenHeight" );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged( [CallerMemberName] string name = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
        }

        private string text = "Open a file to check.";
        private Brush textColour = Brushes.Black;
        private string filePath = null;
        private string fileText = null;
        private Config configFileData;
    }
}
