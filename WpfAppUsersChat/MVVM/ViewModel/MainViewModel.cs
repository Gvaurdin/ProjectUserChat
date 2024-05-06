using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WpfAppUsersChat.Core;
using WpfAppUsersChat.MVVM.Core;
using WpfAppUsersChat.Net;

namespace WpfAppUsersChat.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        static bool isConnecting = false;

        public RelayCommand ConncetToServerCommand { get; set; }

        private Server _server { get; set; }

        public RelayCommand SendCommand { get; set; }


        private string _buttonText;

        public string ButtonText
        {
            get { return _buttonText; }
            set 
            {
                _buttonText = value;
                OnPropertyChanged();
            }
        }

        private SolidColorBrush _buttonColor;

        public SolidColorBrush ButtonColor
        {
            get { return _buttonColor; }
            set 
            {
                _buttonColor = value;
                OnPropertyChanged();
            }
        }





        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }


        
        public MainViewModel()
        {
            ButtonColor = new SolidColorBrush(Colors.Green);
            ButtonText = "Connect";
            ConncetToServerCommand = new RelayCommand(o => Connect());

        }

        private void Connect()
        {
            try
            {
                if (!isConnecting)
                {
                    _server = new Server();
                    if (!_server._client.Connected)
                    {
                        //_server.ConnectToServer();
                        ButtonText = "Disconnected";
                        ButtonColor = new SolidColorBrush(Colors.Red);
                        isConnecting = true;
                    }
                }
                else if (isConnecting)
                {
                    ButtonText = "Connect";
                    ButtonColor = new SolidColorBrush(Colors.Green);
                    isConnecting = false;
                    if (_server._client.Connected)
                    {
                        _server._client.GetStream().Close();
                        _server._client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message, "Server", MessageBoxButton.OK);
            }   
        }
    }
}
