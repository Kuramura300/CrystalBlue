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
using CrystalBlue.HOCONValidator.Validation;
using CrystalBlue.HOCONValidator.ViewModels;

namespace CrystalBlue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// View Model for the HOCON Validator
        /// </summary>
        public HOCONValidatorViewModel ValidatorViewModel { get; set; } = new HOCONValidatorViewModel();
    }
}
