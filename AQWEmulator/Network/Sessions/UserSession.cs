namespace AQWEmulator.Network.Sessions
{
    public class UserSession
    {
        private readonly Session _session;
        
        public string Address => _session.Address;
        
        public UserSession(Session session)
        {
            _session = session;
        }

        public void Send(byte[] data)
        {
            _session.Send(data);
        }
    }
}