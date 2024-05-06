using MessageProcessingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppUsersChat.Core;
using WpfAppUsersChat.MVVM.Core;
using WpfAppUsersChat.Net;
using static MessageProcessingLibrary.PacketReader;

namespace WpfAppUsersChat.MVVM.ViewModel
{
    class VerificationViewModel : ObservableObject
    {
        private bool checkReg;
        public bool CheckReg
        {
            get { return checkReg; }
            set
            {
                checkReg = value;
                OnPropertyChanged();
            }

        }

        private bool checkLog;
        public bool CheckLog
        {
            get { return checkLog; }
            set
            {
                checkLog = value;
                OnPropertyChanged();
            }

        }


        private bool enableConnection;

        public bool EnableConnection
        {
            get { return enableConnection; }
            set
            {
                enableConnection = value;
                OnPropertyChanged();
            }
        }

        private Visibility controlVisibility;

        public Visibility ControlVisibility
        {
            get { return controlVisibility; }
            set
            {
                controlVisibility = value;
                OnPropertyChanged();
            }
        }

        private string nameTextBox;

        public string NameTextBox
        {
            get { return nameTextBox; }
            set
            {
                nameTextBox = value;
                OnPropertyChanged();
            }
        }

        private string textPassBox;

        public string TextPassBox
        {
            get { return textPassBox; }
            set
            {
                textPassBox = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand Verification { get; set; }

        bool isOKVerification = false;

        Server _server;
        public VerificationViewModel()
        {
            _server = new Server();
            enableConnection = false;
            Verification = new RelayCommand(o => VerificationConnection());
        }

        private async void VerificationConnection()
        {
            try
            {
                if (NameTextBox != "" && TextPassBox.Length > 4 && TextPassBox != "")
                {
                    if (checkReg == true)
                    {
                        RegistrationMessage registrationMessage = new RegistrationMessage(nameTextBox, textPassBox, "Registration");
                        MessageGarbage messageGarbage = new MessageGarbage(registrationMessage);
                        PacketBuilder packetBuilder = new PacketBuilder();
                        packetBuilder.WriteOpCode(messageGarbage._opCode);
                        packetBuilder.WriteMessageGarbage(messageGarbage);
                        _server.ConnectToServer(packetBuilder.GetPacketBytes());
                        byte[] response = await _server.WaitForResponseAsync();
                        string stringResponse = Encoding.ASCII.GetString(response);
                        if(stringResponse == "Ok")
                        {
                            EnableConnection = true;
                            ControlVisibility = Visibility.Hidden;
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            Application.Current.MainWindow.Close();

                        }
                        else 
                        {
                            MessageBox.Show("Change name or password");
                        }
                    }
                    else if (checkLog == true)
                    {
                        LoginMessage loginMessage = new LoginMessage(nameTextBox, textPassBox, "Login");
                        MessageGarbage messageGarbage = new MessageGarbage(loginMessage);
                        PacketBuilder packetBuilder = new PacketBuilder();
                        packetBuilder.WriteOpCode(messageGarbage._opCode);
                        packetBuilder.WriteMessageGarbage(messageGarbage);
                        _server.ConnectToServer(packetBuilder.GetPacketBytes());
                        byte[] response = await _server.WaitForResponseAsync();
                    }
                }
                else
                {
                    MessageBox.Show($"Enter to name and password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.Message}");
            }
        }
    }
}
