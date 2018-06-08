using System.Collections.Concurrent;
using AQWEmulator.World.Parties;
using AQWEmulator.World.Users;

namespace AQWEmulator.Helper
{
    public class PartyHelper
    {
        private static PartyHelper instance;
        private readonly object Lock = new object();
        private readonly ConcurrentDictionary<long, PartyInfo> Parties = new ConcurrentDictionary<long, PartyInfo>();
        private long LastPartyId = 1;

        public static PartyHelper Instance => instance ?? (instance = new PartyHelper());

        public int Count
        {
            get
            {
                lock (Lock)
                {
                    return Parties.Count;
                }
            }
        }

        public long GeneratePartyId()
        {
            return LastPartyId++;
        }

        private bool TryGetPartyInfo(int partyId, out PartyInfo party)
        {
            lock (Lock)
            {
                return Parties.TryGetValue(partyId, out party);
            }
        }

        public void Create(User user)
        {
            var party = new PartyInfo();
            lock (Lock)
            {
                if (Parties.TryAdd(LastPartyId, party))
                {
                }
            }
        }

        public void Add(int partyId, User user)
        {
            if (user.PartyId <= 0 && TryGetPartyInfo(partyId, out var party))
            {
            }
        }
    }
}