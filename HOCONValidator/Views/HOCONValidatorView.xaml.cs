using System;
using System.Collections.Generic;
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

namespace CrystalBlue.HOCONValidator.Views
{
    /// <summary>
    /// Interaction logic for HOCONValidatorView.xaml
    /// </summary>
    public partial class HOCONValidatorView : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HOCONValidatorView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Width of the window used for scroll viewer auto showing scrollbars
        /// </summary>
        public double ScreenWidth
        {
            get
            {
                return this.ActualWidth;
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
    }
}
