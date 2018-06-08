using System;

namespace AQWEmulator.Network
{
    internal sealed class SendDataToken
    {
        public int SendBytesRemainingCount { get; set; }

        public int BytesSentAlreadyCount { get; set; }

        public byte[] DataToSend { get; set; }

        public void Reset()
        {
            SendBytesRemainingCount = 0;
            BytesSentAlreadyCount = 0;
            Array.Clear(DataToSend, 0, DataToSend.Length);
            DataToSend = null;
        }
    }
}