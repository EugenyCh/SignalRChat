#region snippet_MainWindowClass

using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace SignalRChatClient
{
    public partial class MainWindow : Window
    {
        private HubConnection connection;

        public MainWindow()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:53353/ChatHub")
                .Build();
                
            #region snippet_ClosedRestart

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            #endregion snippet_ClosedRestart
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            #region snippet_ConnectionOn

            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}";
                    messagesList.Items.Add(newMessage);
                });
            });

            #endregion snippet_ConnectionOn

            try
            {
                await connection.StartAsync();
                messagesList.Items.Add("Connection started");
                connectButton.IsEnabled = false;
                sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            #region snippet_ErrorHandling

            try
            {
                #region snippet_InvokeAsync

                await connection.InvokeAsync("SendMessage",
                    userTextBox.Text, messageTextBox.Text);

                #endregion snippet_InvokeAsync
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }

            #endregion snippet_ErrorHandling
        }
    }
}

#endregion