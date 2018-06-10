using System;

namespace AQWEmulator.Attributes
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