using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using AQWEmulator.Database;
using AQWEmulator.Network;
using AQWEmulator.Network.Packet;
using AQWEmulator.World.Core;
using AQWEmulator.Settings;
using AQWEmulator.Utils.Exceptions;
using AQWEmulator.Utils.Files;
using AQWEmulator.Utils.Log;
using AQWEmulator.World;

namespace AQWEmulator
{
    public static class Emulator
    {
        public const string PrettyName = "Hikari";
        private const string PrettyVersion = "Hikari Emulator";
        private const int Major = 0;
        private const int Minor = 14;
        private const int Build = 1;

        private static NetworkServer _network;

        public static void Main(string[] args)
        {
            ThreadPool.SetMaxThreads(200, 200);
            var start = DateTime.Now;
            try
            {
                var coreStart = DateTime.Now;
                var xml = Xml.Load(Path.Combine(Application.StartupPath, @"data/Settings.xml"));
                ServerCoreValues.LoadCoreSettings(
                    Path.Combine(Application.StartupPath, @"data/core/ServerSettings.xml"), "name", "value");
                ServerCoreValues.LoadCoreValues(
                    Path.Combine(Application.StartupPath, @"data/core/ServerCoreValues.xml"), "name", "value");
                ServerCoreValues.LoadCoreLevels(Path.Combine(Application.StartupPath, @"data/core/ServerLevel.xml"),
                    "level", "rate", "value");
                var coreUsed = DateTime.Now - coreStart;
                WriteConsole.Info($"Core cached in {coreUsed.Seconds} s, {coreUsed.Milliseconds}");
                var serverId = int.Parse(Xml.ToString(xml.SelectNodes("/hikari/@serverID")));
                var _host = Xml.ToString(xml.SelectNodes("/hikari/network/host"));
                var _port = int.Parse(Xml.ToString(xml.SelectNodes("/hikari/network/port")));
                ServerCoreValues.LoadSettings(new ServerSettings
                {
                    LimitGold = int.Parse(Xml.ToString(xml.SelectNodes("/hikari/limits/gold"))),
                    LimitCoin = int.Parse(Xml.ToString(xml.SelectNodes("/hikari/limits/coin"))),
                    GoldRate = int.Parse(Xml.ToString(xml.SelectNodes("/hikari/rates/gold"))),
                    CoinRate = int.Parse(Xml.ToString(xml.SelectNodes("/hikari/rates/coin"))),
                    ExpRate = int.Parse(Xml.ToString(xml.SelectNodes("/hikari/rates/experience"))),
                    RepRate = int.Parse(Xml.ToString(xml.SelectNodes("/hikari/rates/reputation"))),
                    ClassRate = int.Parse(Xml.ToString(xml.SelectNodes("/hikari/rates/class")))
                });
                var gameDatabaseStart = DateTime.Now;
                GameFactory.Build(new SessionSettings
                {
                    Host = Xml.ToString(xml.SelectNodes("/hikari/sql/host")),
                    Username = Xml.ToString(xml.SelectNodes("/hikari/sql/username")),
                    Password = Xml.ToString(xml.SelectNodes("/hikari/sql/password")),
                    Database = Xml.ToString(xml.SelectNodes("/hikari/sql/database")),
                    Port = int.Parse(Xml.ToString(xml.SelectNodes("/hikari/sql/port")))
                });
                var gameDatabaseUsed = DateTime.Now - gameDatabaseStart;
                Server.Instance.Cache();
                PacketProcessor.Register();
                WriteConsole.Info($"Game database {gameDatabaseUsed.Seconds} s, {gameDatabaseUsed.Milliseconds}");
                _network = new NetworkServer().Build(new NetworkSettings()
                {
                    Endpoint = new IPEndPoint(IPAddress.Any, 5588),
                    BufferSize = 1024,
                    Backlog = 5,
                    MaxSimultaneousAcceptOps = 512,
                    NumOfSaeaForRec = 5000,
                    NumOfSaeaForSend = 5000
                }).Init();
                _network.StartListen();
                var used = DateTime.Now - start;
                WriteConsole.Info($"Server started in {used.Seconds} s, {used.Milliseconds} ms on {_host}:{_port}");
                while (true)
                {
                    Thread.Sleep(100);
                }
            }
            catch (FileNotFound)
            {
                Console.ReadKey(true);
                Environment.Exit(1);
            }
        }
    }
}