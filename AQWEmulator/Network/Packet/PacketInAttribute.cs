using System;

namespace AQWEmulator.Network.Packet
{
    internal class PacketInAttribute : Attribute
    {
        public string Packet { get; }
    
        public PacketInAttribute(string packet)
        {
            Packet = packet;
        }
    }
}