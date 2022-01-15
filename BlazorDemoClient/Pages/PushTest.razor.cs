using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Pages
{
    public partial class PushTest : KenovaDialogBase, IAsyncDisposable
    {
        private HubConnection hubConnection;
        private ObservableCollection<PushTestData> messages = new();

        //private string userInput;
        //private string messageInput;

        private PushTestData _selectedMessage = null;

        private ToolbarItemCollection toolbar = new();

        public PushTestModel Model = new();
        private HyperData<PushTestData> Data = new();
        private HyperGrid<PushTestData> _hypergrid;

        protected override async Task OnDialogInitializedAsync()
        {
            toolbar.SourceCodeButton("Pages");

            Data.Items = messages;
            Data.Mode = DisplayMode.Virtualization;
            Data.SelectedItemExpression = () => this._selectedMessage;

            Data.Columns.Add(c => c.ReceivedTime, "Time", 100, false);
            Data.Columns.Add(c => c.User, "User", 200, false);
            Data.Columns.Add(c => c.Message, "Message", 300, false);
            Data.Columns.AddIcon((data) => GetIcon(data));

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var data = new PushTestData
                {
                    Received = DateTime.Now,
                    User = user,
                    Message = $"{user}: {message}"
                };

                messages.Add(data);
            });

            await hubConnection.StartAsync();
        }

        private IconDefinition GetIcon(PushTestData data)
        {
            var icon = new IconDefinition { IconKind = IconKind.FontAwesome, IconData = "fas fa-box" };

            return icon;

        }

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }


        public bool IsConnected
        {
            get { return hubConnection.State == HubConnectionState.Connected; }
        }

        private async Task SendMessageClickedAsync()
        {
            await hubConnection.SendAsync("SendMessage", "demo", this.Model.Message);
        }


    }

    public class PushTestData
    {
        public DateTime Received;
        public string User;
        public string Message;

        public string ReceivedTime
        {
            get { return Received.ToLongTimeString(); }
        }
    }

}
