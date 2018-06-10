using AQWEmulator.Attributes;
using AQWEmulator.Utils;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("aggroMon")]
    public class AggroMonster : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            if (!user.RoomUser.RoomMonsterManager.TryGet(int.Parse(parameters[0]), out var monster)) return;
            var monsterState = monster.MonsterState;
            monsterState.AddTarget(user);
            if (monsterState.IsNeutral)
            {
                monsterState.SetState(Indent.State.IntCombat);
            }
        }
    }
}