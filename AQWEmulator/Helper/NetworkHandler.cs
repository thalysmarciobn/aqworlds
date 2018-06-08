using System.Net.Sockets;
using AQWEmulator.World.Users;

namespace AQWEmulator.Handler
{
    public delegate void OnAccept(Socket socket);

    public delegate void OnReceive(User connection, string data);
}