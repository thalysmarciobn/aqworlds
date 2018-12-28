using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using AQWEmulator.Database;
using AQWEmulator.Network;
using AQWEmulator.Network.Packet;
using AQWEmulator.World.Core;
using AQWEmulator.Utils.Exceptions;
using AQWEmulator.Utils.Log;
using AQWEmulator.World;
using AQWEmulator.Xml;
using AQWEmulator.Xml.Game;
using AQWEmulator.Xml.Game.Core;

namespace AQWEmulator
{
    public static class Emulator
    {
        public const string PrettyName = "Hikari";
        private const string PrettyVersion = "Hikari Emulator";
        private const int Major = 0;
        private const int Minor = 14;
        private const int Build = 1;
        
        public static readonly XmlSettingsSerializer<ServerSettings> Settings = new XmlSettingsSerializer<ServerSettings>();

        public static void Main(string[] args)
        {
            var start = DateTime.Now;
            try
            {
                var coreStart = DateTime.Now;
                if (Settings.Load(Path.Combine(Application.StartupPath, @"data/Settings.xml")))
                {
                    ServerCoreValues.ExpTable.Load(Path.Combine(Application.StartupPath, @"data/core/ExpTable.xml"));
                    ServerCoreValues.Values.Load(Path.Combine(Application.StartupPath, @"data/core/Values.xml"));
                    ServerCoreValues.Settings.Load(Path.Combine(Application.StartupPath, @"data/core/Settings.xml"));
                    var coreUsed = DateTime.Now - coreStart;
                    WriteConsole.Info($"Core cached in {coreUsed.Seconds} s, {coreUsed.Milliseconds} ms");


                    var gameDatabaseStart = DateTime.Now;

                    if (GameFactory.Build(Settings.Configuration.ServerDatabase, false))
                    {
                        var gameDatabaseUsed = DateTime.Now - gameDatabaseStart;
                        WriteConsole.Info(
                            $"Game database {gameDatabaseUsed.Seconds} s, {gameDatabaseUsed.Milliseconds}");
                        var networkServer = new NetworkServer(Settings.Configuration.ServerNetwork);
                        Server.Instance.Cache();
                        PacketProcessor.Register();
                        var used = DateTime.Now - start;
                        try
                        {
                            networkServer.Bind();
                            WriteConsole.Info(
                                $"Server started in {used.Seconds} s, {used.Milliseconds} ms on {networkServer.IpEndPoint}");
                        }
                        catch
                        {
                            WriteConsole.Error($"Can't start server on {networkServer.IpEndPoint}");
                        }
                    }
                }

                while (true)
                {
                    Thread.Sleep(50);
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