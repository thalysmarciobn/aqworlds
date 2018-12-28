using System;
using System.Net.Sockets;
using System.Text;
using AQWEmulator.Database;
using AQWEmulator.Database.Models;
using AQWEmulator.Helper;
using AQWEmulator.Network.Packet;
using AQWEmulator.Utils;
using AQWEmulator.Utils.AQW;
using AQWEmulator.World;
using AQWEmulator.World.Users;
using AQWEmulator.Xml;

namespace AQWEmulator.Network.Sessions
{
    public class Session
    {
        private readonly SocketAsyncEventArgs _acceptEventArg;

        private readonly NetworkServer _networkServer;

        private readonly Socket _socket;
        
        public string Address => _socket.RemoteEndPoint.ToString().Split(':')[0];

        public Session(SocketAsyncEventArgs acceptEventArg, Socket socket, NetworkServer networkServer)
        {
            _acceptEventArg = acceptEventArg;
            _socket = socket;
            _networkServer = networkServer;
        }

        public void Send(string message)
        {
            Console.WriteLine(message);
            _networkServer.SendData(_socket, Encoding.UTF8.GetBytes(message + Convert.ToChar(0x0)));
        }

        public void Shutdown()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public bool ReceiveAsync(SocketAsyncEventArgs receiveEventArgs)
        {
            return _socket.ReceiveAsync(receiveEventArgs);
        }

        public void Receive(string data)
        {
            if (data.Equals("<policy-file-request/>" + Convert.ToChar(0x0)))
            {
                Send($"<cross-domain-policy><allow-access-from domain='*' to-ports='{Emulator.Settings.Configuration.ServerNetwork.Port}' /></cross-domain-policy>");
            }
            else if (XmlUtils.IsValidXml(data))
            {
                var action = XmlUtils.SelectSingleNode(data, "/msg/body/@action").Value;
                if (action.Equals("verChk"))
                {
                    Send("<msg t='sys'><body action='apiOK' r='0'></body></msg>");
                }
                else if (action.Equals("login"))
                {
                    var nick = XmlUtils.ToString(XmlUtils.SelectNodes(data, "/msg/body/login/nick"));
                    var hash = XmlUtils.ToString(XmlUtils.SelectNodes(data, "/msg/body/login/pword"));
                    var name = nick.Replace(Indent.Key, "");
                    if (!UsersManager.Instance.ContainsUserByName(name))
                    {
                        using (var session = GameFactory.Session.OpenSession())
                        {
                            using (var transaction = session.BeginTransaction())
                            {
                                var userModel = session.QueryOver<UserModel>().Where(x => x.Name == name)
                                    .SingleOrDefault();
                                if (userModel != null)
                                {
                                    userModel.LastAccess = DateTime.Now;
                                    GameFactory.Update(userModel);
                                    var character = session.QueryOver<CharacterModel>()
                                        .Where(x => x.UserId == userModel.Id).Where(x => x.Token == hash)
                                        .SingleOrDefault();
                                    if (character == null) return;
                                    var user = UsersManager.Instance.AddAndGet(name, this, character);
                                    var access = new AccessLogModel
                                    {
                                        UserId = character.UserId,
                                        Address = Address,
                                        CharId = character.Id,
                                        Date = DateTime.Now
                                    };
                                    session.Save(access);
                                    transaction.Commit();
                                    user.Character.Token = "Nemesis-" + Token.Create(32);
                                    character.Address = Address;
                                    //character.Country = Emulator.GeoIP.GetCountryCode(context.Channel.IpAddress);
                                    session.Update(character);

                                    NetworkHelper.SendResponse(
                                        Preference.Is(Indent.Preference.Party, character.Settings)
                                            ? new[] {"xt", "server", "-1", Indent.Message.PartyOn}
                                            : new[] {"xt", "warning", "-1", Indent.Message.PartyOff}, user);
                                    NetworkHelper.SendResponse(
                                        Preference.Is(Indent.Preference.Goto, character.Settings)
                                            ? new[] {"xt", "server", "-1", Indent.Message.GotoOn}
                                            : new[] {"xt", "server", "-1", Indent.Message.GotoOff}, user);
                                    NetworkHelper.SendResponse(
                                        Preference.Is(Indent.Preference.Friend, character.Settings)
                                            ? new[] {"xt", "server", "-1", Indent.Message.FriendOn}
                                            : new[] {"xt", "server", "-1", Indent.Message.FriendOff}, user);
                                    NetworkHelper.SendResponse(
                                        Preference.Is(Indent.Preference.Whisper, character.Settings)
                                            ? new[] {"xt", "server", "-1", Indent.Message.WhisperOn}
                                            : new[] {"xt", "server", "-1", Indent.Message.WhisperOff}, user);
                                    NetworkHelper.SendResponse(
                                        Preference.Is(Indent.Preference.Tooltips, character.Settings)
                                            ? new[] {"xt", "server", "-1", Indent.Message.TooltipsOn}
                                            : new[] {"xt", "server", "-1", Indent.Message.TooltipsOff}, user);
                                    NetworkHelper.SendResponse(
                                        Preference.Is(Indent.Preference.Duel, character.Settings)
                                            ? new[] {"xt", "server", "-1", Indent.Message.DuelOn}
                                            : new[] {"xt", "server", "-1", Indent.Message.DuelOff}, user);
                                    NetworkHelper.SendResponse(
                                        Preference.Is(Indent.Preference.Guild, character.Settings)
                                            ? new[] {"xt", "server", "-1", Indent.Message.GuildOn}
                                            : new[] {"xt", "server", "-1", Indent.Message.GuildOff}, user);
                                    NetworkHelper.SendResponse(
                                        new[]
                                        {
                                            "xt", "loginResponse", "-1", "true", user.Id.ToString(), user.Name,
                                            "MOTD",
                                            DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"),
                                            Server.Instance.SettingsBuilder.ToString()
                                        }, user);
                                    _acceptEventArg.UserToken = user;
                                }
                                else
                                {
                                    Send(SmartFoxServer.Parse(new[]
                                    {
                                        "xt", "loginResponse", "-1", "false", "-1", name,
                                        $"User Data for {name} could not be retrieved. Please contact the staff to resolve the issue."
                                    }));
                                }
                            }
                        }
                    }
                    else
                    {
                        Send(SmartFoxServer.Parse(new[]
                        {
                            "xt", "loginResponse", "-1", "false", "-1", name,
                            $"User Data for {name} could not be retrieved. Please contact the staff to resolve the issue."
                        }));
                    }
                }
            }
        }
    }
}