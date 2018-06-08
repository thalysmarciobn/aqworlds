using System;
using System.Net.Sockets;
using System.Text;
using AQWEmulator.Database;
using AQWEmulator.Database.Models;
using AQWEmulator.Helper;
using AQWEmulator.Network.Packet;
using AQWEmulator.Utils;
using AQWEmulator.Utils.AQW;
using AQWEmulator.Utils.Files;
using AQWEmulator.World;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Sessions
{
    public class Session
    {
        private int Id { get; }
        
        public bool Logged { get; private set; }
        
        public Socket Socket { get; set; }

        private User _user;
        
        public UserSession UserSession { get; private set; }

        private readonly NetworkServer _networkServer;
        
        public string Address => Socket.RemoteEndPoint.ToString().Split(':')[0];

        public Session(int id, NetworkServer networkServer)
        {
            Id = id;
            Logged = false;
            _networkServer = networkServer;
        }

        public void Disconnect()
        {
            if (!Logged) return;
            _user.RoomUser.Remove();
            UsersManager.Instance.Remove(_user);
            UserSession = null;
            Logged = false;
        }

        public void UserReceive(string packet)
        {
            var dataArray = packet.Substring(0, packet.Length - 1).Split(Convert.ToChar(0x0));
            foreach (var message in dataArray)
            {
                var _params = message.Substring(1, message.Length - 2).Split('%');
                if (_params.Length <= 3) return;
                var newParams = new string[_params.Length - 4];
                if (!_params[0].Equals("xt") || !_params[1].Equals("zm")) return;
                var cmd = _params[2];
                if (!int.TryParse(_params[3], out var room)) room = 0;
                Array.Copy(_params, 4, newParams, 0, _params.Length - 4);
                PacketProcessor.TryHandlePacket(_user, cmd, room, newParams);
                //if (TryGet(cmd, out var packetEvent))
                //    packetEvent.Dispatch(user, room, newParams);
                //else if (TryGetCustom(cmd, out var custom))
                //    custom.Parser(user, room, newParams);
                //else
                //    Console.WriteLine($"[{user.Name}] Packet not found: {cmd}");
            }
        }

        public void Receive(string data)
        {
            try
            {
                if (data.Equals("<policy-file-request/>" + Convert.ToChar(0x0)))
                {
                    Send(Encoding.UTF8.GetBytes(
                        $"<cross-domain-policy><allow-access-from domain='*' to-ports='{5588}' /></cross-domain-policy>"
                        + Convert.ToChar(0x0)));
                }
                else if (Xml.IsValidXml(data))
                {
                    var action = Xml.SelectSingleNode(data, "/msg/body/@action").Value;
                    if (action.Equals("verChk"))
                    {
                        Send(Encoding.UTF8.GetBytes(
                            "<msg t='sys'><body action='apiOK' r='0'></body></msg>" + Convert.ToChar(0x0)));
                    }
                    else if (action.Equals("login"))
                    {
                        var nick = Xml.ToString(Xml.SelectNodes(data, "/msg/body/login/nick"));
                        var hash = Xml.ToString(Xml.SelectNodes(data, "/msg/body/login/pword"));
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
                                        Logged = true;
                                        UserSession = new UserSession(this);
                                        _user = UsersManager.Instance.AddAndGet(name, UserSession, character);
                                        var access = new AccessLogModel
                                        {
                                            UserId = character.UserId,
                                            Address = Address,
                                            CharId = character.Id,
                                            Date = DateTime.Now
                                        };
                                        session.Save(access);
                                        transaction.Commit();
                                        _user.Character.Token = "Nemesis-" + Token.Create(32);
                                        character.Address = Address;
                                        //character.Country = Emulator.GeoIP.GetCountryCode(context.Channel.IpAddress);
                                        session.Update(character);

                                        NetworkHelper.SendResponse(
                                            Preference.Is(Indent.Preference.Party, character.Settings)
                                                ? new[] {"xt", "server", "-1", Indent.Message.PartyOn}
                                                : new[] {"xt", "warning", "-1", Indent.Message.PartyOff}, _user);
                                        NetworkHelper.SendResponse(
                                            Preference.Is(Indent.Preference.Goto, character.Settings)
                                                ? new[] {"xt", "server", "-1", Indent.Message.GotoOn}
                                                : new[] {"xt", "server", "-1", Indent.Message.GotoOff}, _user);
                                        NetworkHelper.SendResponse(
                                            Preference.Is(Indent.Preference.Friend, character.Settings)
                                                ? new[] {"xt", "server", "-1", Indent.Message.FriendOn}
                                                : new[] {"xt", "server", "-1", Indent.Message.FriendOff}, _user);
                                        NetworkHelper.SendResponse(
                                            Preference.Is(Indent.Preference.Whisper, character.Settings)
                                                ? new[] {"xt", "server", "-1", Indent.Message.WhisperOn}
                                                : new[] {"xt", "server", "-1", Indent.Message.WhisperOff}, _user);
                                        NetworkHelper.SendResponse(
                                            Preference.Is(Indent.Preference.Tooltips, character.Settings)
                                                ? new[] {"xt", "server", "-1", Indent.Message.TooltipsOn}
                                                : new[] {"xt", "server", "-1", Indent.Message.TooltipsOff}, _user);
                                        NetworkHelper.SendResponse(
                                            Preference.Is(Indent.Preference.Duel, character.Settings)
                                                ? new[] {"xt", "server", "-1", Indent.Message.DuelOn}
                                                : new[] {"xt", "server", "-1", Indent.Message.DuelOff}, _user);
                                        NetworkHelper.SendResponse(
                                            Preference.Is(Indent.Preference.Guild, character.Settings)
                                                ? new[] {"xt", "server", "-1", Indent.Message.GuildOn}
                                                : new[] {"xt", "server", "-1", Indent.Message.GuildOff}, _user);
                                        NetworkHelper.SendResponse(
                                            new[]
                                            {
                                                "xt", "loginResponse", "-1", "true", _user.Id.ToString(), _user.Name,
                                                "MOTD",
                                                DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"),
                                                Server.Instance.SettingsBuilder.ToString()
                                            }, _user);
                                    }
                                    else
                                    {
                                        Send(Encoding.UTF8.GetBytes(SmartFoxServer.Parse(new[]
                                        {
                                            "xt", "loginResponse", "-1", "false", "-1", name,
                                            $"User Data for {name} could not be retrieved. Please contact the staff to resolve the issue."
                                        }) + Convert.ToChar(0x0)));
                                    }
                                }
                            }
                        }
                        else
                        {
                            Send(Encoding.UTF8.GetBytes(SmartFoxServer.Parse(new[]
                            {
                                "xt", "loginResponse", "-1", "false", "-1", name,
                                $"User Data for {name} could not be retrieved. Please contact the staff to resolve the issue."
                            }) + Convert.ToChar(0x0)));
                        }
                    }
                }
            }
            catch
            {
                
            }

            //Console.WriteLine(data);
        }

        public void Send(byte[] data)
        {
            _networkServer.SendData(Socket, data);
        }
    }
}