using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppUsersChat.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для WindowVerification.xaml
    /// </summary>
    public partial class WindowVerification : Window
    {
        public static WindowVerification _window { get; set; }
        public WindowVerification()
        {
            InitializeComponent();
            _window = this;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MovingWindows(object sender, RoutedEventArgs e) 
        {
            if(Mouse.LeftButton == MouseButtonState.Pressed)
            {
                WindowVerification._window.DragMove();
            }
        }

    }
}
