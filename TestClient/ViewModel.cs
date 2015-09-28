using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using EasyPacketSharp.Abstract;
using SharedChatHandlers;

namespace ChatClient
{
    public class ViewModel : ExtendedNotificationBase
    {
        private List<IClient> Clients { get; } = new List<IClient>();
        public EventHandler OnMessage;

        public DelegateCommand<string> Connect
        {
            get
            {
                return Get(() => Connect, new DelegateCommand<string>
                {
                    CanExecute = s => string.IsNullOrEmpty(s),
                    Execute = s =>
                    {
                        Task.Run(() =>
                        {
                            var client = new Client();
                            try
                            {
                                client.OnPacket += (sock, p) => OnPacket(client, p);
                                client.OnDisconnection += sock => Clients.Remove(client);
                                client.Connect(s);
                            }
                            catch (Exception)
                            {
                                return;
                            }

                            client.SendPacket(SharedHandlers.SendName("Joachim"));
                            Clients.Add(client);
                        });
                    }
                });
            }
        }

        public DelegateCommand<string> SendMessage
        {
            get
            {
                return Get(() => SendMessage, new DelegateCommand<string>
                {
                    CanExecute = s => string.IsNullOrEmpty(s),
                    Execute = s =>
                    {
                        Messages.Add(new MessageModel
                        {
                            Message = s,
                            Owner = "Joachim",
                            Self = true
                        });
                        OnMessage?.Invoke(this, null);

                        foreach (var client in Clients)
                            client.SendPacket(SharedHandlers.SendMessage(s));
                    }
                });
            }
        }

        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();

        private void OnPacket(Client c, IImmutablePacket p)
        {
            switch ((OperationCode)p.ReadUShort())
            {
                case OperationCode.SendMessage:
                    Application.Current.Dispatcher.InvokeAsync(
                        () =>
                        {
                            Messages.Add(new MessageModel
                            {
                                Message = SharedHandlers.GetMessage(p),
                                Owner = c.Name,
                                Self = false
                            });
                            OnMessage?.Invoke(this, null);
                        }
                    );

                    break;
                case OperationCode.SetName:
                    c.Name = SharedHandlers.GetName(p);
                    break;
            }

        }
    }
}
