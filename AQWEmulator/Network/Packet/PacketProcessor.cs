using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using AQWEmulator.Attributes;
using AQWEmulator.Utils.Log;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet
{
    public static class PacketProcessor
    {
        private static readonly ConcurrentDictionary<string, IPacketHandler> Events = new ConcurrentDictionary<string, IPacketHandler>();

        public static void Register()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsClass || type.GetInterface(typeof(IPacketHandler).FullName) == null) continue;
                var packethandlerattribs =
                    (PacketInAttribute[]) type.GetCustomAttributes(typeof(PacketInAttribute), true);
                if (packethandlerattribs.Length <= 0) continue;
                if (packethandlerattribs[0].Packet == null)
                {
                    WriteConsole.Info($"Packet {type} is handling an invalid code.");
                    continue;
                }
                if (!Events.TryAdd(packethandlerattribs[0].Packet, (IPacketHandler) Activator.CreateInstance(type)))
                {
                    WriteConsole.Info($"Packet {type} is repeating the code of other packet ({packethandlerattribs[0].Packet}).");
                }
            }
            WriteConsole.Info($"{Events.Count} packets loaded.");
        }

        public static void TryHandlePacket(User user, string packet, int room, string[] parameters)
        {
            if (Events.TryGetValue(packet, out var packetHandler))
            {
                packetHandler.Dispatch(user, room, parameters);
            }
        }
    }
}