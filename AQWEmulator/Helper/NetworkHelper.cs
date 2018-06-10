using System.Collections.Generic;
using AQWEmulator.Utils;
using AQWEmulator.World.Rooms;
using AQWEmulator.World.Users;
using JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AQWEmulator.Helper
{
    public static class NetworkHelper
    {
        public static void SendResponse(string packet, RoomUser roomUser)
        {
            foreach (var client in roomUser.RoomUserManager.Users)
                client.Session.Send(packet);
        }

        public static void SendResponse(string packet, RoomUserManager roomUserManager)
        {
            foreach (var client in roomUserManager.Users)
                client.Session.Send(packet);
        }

        public static void SendResponseToOthers(string data, RoomUser roomUser)
        {
            foreach (var client in roomUser.RoomUserManager.Users)
                if (client.UserId != roomUser.UserId)
                    client.Session.Send(data);
        }

        public static void SendResponse(string[] data, User user)
        {
            user.Session.Send(SmartFoxServer.Parse(data));
        }

        public static void SendResponse(string[] data, IEnumerable<User> users)
        {
            var send = SmartFoxServer.Parse(data);
            foreach (var client in users) client.Session.Send(send);
            ;
        }

        public static void SendResponse(string[] data, RoomUser roomUser)
        {
            var send = SmartFoxServer.Parse(data);
            foreach (var client in roomUser.RoomUserManager.Users)
                client.Session.Send(send);
        }

        public static void SendResponse(string[] data, RoomUserManager roomUserManager)
        {
            var send = SmartFoxServer.Parse(data);
            foreach (var client in roomUserManager.Users)
                client.Session.Send(send);
        }

        public static void SendResponseToOthers(string[] data, RoomUser roomUser)
        {
            var send = SmartFoxServer.Parse(data);
            foreach (var client in roomUser.RoomUserManager.Users)
                if (client.UserId != roomUser.UserId)
                    client.Session.Send(send);
        }

        public static void SendResponse(JsonPacket packet, User user)
        {
            user.Session.Send(JsonConvert.SerializeObject(packet, Formatting.None));
        }

        public static void SendResponse(JsonPacket packet, RoomUser roomUser)
        {
            var send = JsonConvert.SerializeObject(packet, Formatting.None);
            foreach (var client in roomUser.RoomUserManager.Users)
                client.Session.Send(send);
        }

        public static void SendResponse(JsonPacket packet, RoomUserManager roomUserManager)
        {
            var send = JsonConvert.SerializeObject(packet, Formatting.None);
            foreach (var client in roomUserManager.Users)
                client.Session.Send(send);
        }

        public static void SendResponseToOthers(JsonPacket data, RoomUser roomUser)
        {
            var send = JsonConvert.SerializeObject(data, Formatting.None);
            foreach (var client in roomUser.RoomUserManager.Users)
                if (client.UserId != roomUser.UserId)
                    client.Session.Send(send);
        }

        public static void SendResponse(JObject packet, User user)
        {
            var b = new JObject {{"r", -1}, {"o", packet}};
            var t = new JObject {{"b", b}, {"t", "xt"}};
            user.Session.Send(t.ToString(Formatting.None));
        }

        public static void SendResponseToOthers(JObject packet, RoomUser roomUser)
        {
            var b = new JObject {{"r", "-1"}, {"o", packet}};
            var t = new JObject {{"b", b}, {"t", "xt"}};
            var data = t.ToString(Formatting.None);
            foreach (var client in roomUser.RoomUserManager.Users)
                if (client.UserId != roomUser.UserId)
                    client.Session.Send(data);
        }
    }
}