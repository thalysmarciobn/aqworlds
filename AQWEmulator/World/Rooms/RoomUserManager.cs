using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Linq;
using AQWEmulator.Helper;
using AQWEmulator.Utils;
using AQWEmulator.Utils.AQW;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.World.Rooms
{
    public class RoomUserManager
    {
        public RoomUserManager(RoomsManager roomsManager, Room room)
        {
            RoomsManager = roomsManager;
            Room = room;
            LocalUsers = new ConcurrentDictionary<int, RoomUser>();
        }

        private Room Room { get; }
        private RoomsManager RoomsManager { get; }
        private ConcurrentDictionary<int, RoomUser> LocalUsers { get; }
        public int Count => Users.Count;
        public ICollection<RoomUser> Users => LocalUsers.Values;

        public void Join(User user)
        {
            if (!LocalUsers.TryAdd(user.Id, user.RoomUser)) return;
            var move = new JsonMoveToArea
            {
                areaId = Room.Id,
                areaName = Room.Name,
                sExtra = "",
                strMapFileName = Room.Area.File,
                strMapName = Room.Area.Name,
                intType = 2,
                uoBranch = new List<JsonUoBranch>(),
                monBranch = new List<JsonMonBranch>()
            };
            var uLs = new XElement("uLs", new XAttribute("r", Room.Id));
            foreach (var roomUser in Users)
            {
                uLs.Add(new XElement("u", new XAttribute("i", roomUser.UserId), new XAttribute("m", 0),
                    new XElement("n", new XCData(roomUser.Name)), new XElement("vars", "")));
                move.uoBranch.Add(new JsonUoBranch
                {
                    entType = "p",
                    strFrame = roomUser.Frame,
                    strPad = roomUser.Pad,
                    strUsername = roomUser.Character.Name,
                    uoName = roomUser.Name,
                    entID = roomUser.UserId,
                    intHP = roomUser.UserState.Health,
                    intHPMax = roomUser.UserState.MaxHealth,
                    intMP = roomUser.UserState.Mana,
                    intMPMax = roomUser.UserState.MaxMana,
                    intLevel = roomUser.Character.Level,
                    intState = roomUser.UserState.State,
                    tx = roomUser.Tx,
                    ty = roomUser.Ty,
                    showCloak = Preference.Is(Indent.Preference.Cloak, roomUser.Character.Settings),
                    showHelm = Preference.Is(Indent.Preference.Helm, roomUser.Character.Settings),
                    afk = roomUser.Afk
                });
            }

            NetworkHelper.SendResponse(
                new XElement("msg", new XAttribute("t", "sys"),
                        new XElement("body", new XAttribute("action", "joinOK"), new XAttribute("r", Room.Id),
                            new XElement("pid", new XAttribute("id", user.Id)), new XElement("vars"), uLs))
                    .ToString(SaveOptions.DisableFormatting), user.RoomUser);
            NetworkHelper.SendResponseToOthers(
                new XElement("msg", new XAttribute("t", "sys"),
                        new XElement("body", new XAttribute("action", "uER"), new XAttribute("r", Room.Id),
                            new XElement("u", new XAttribute("i", user.Id), new XAttribute("m", 0),
                                new XAttribute("s", 0), new XAttribute("p", user.Id),
                                new XElement("n", new XCData(user.Name)), new XElement("vars", ""))))
                    .ToString(SaveOptions.DisableFormatting), user.RoomUser);

            if (Room.MonsterManager.Count > 0)
            {
                move.mondef = new List<JsonMonsterDefinition>();
                move.monmap = new List<JsonMonMap>();
                foreach (var monsterAi in Room.MonsterManager.Monsters)
                {
                    var monsterState = monsterAi.MonsterState;
                    var monsterModel = monsterAi.MonsterModel;
                    var areaMonster = monsterAi.AreaMonsterModel;
                    move.monBranch.Add(new JsonMonBranch
                    {
                        MonID = monsterModel.Id.ToString(),
                        MonMapID = areaMonster.MonMapId.ToString(),
                        bRed = 0,
                        iLvl = monsterModel.Level,
                        intHP = monsterModel.Heath,
                        intHPMax = monsterModel.Heath,
                        intMP = monsterModel.Mana,
                        intMPMax = monsterModel.Mana,
                        intState = monsterState.State,
                        wDPS = monsterModel.Dps
                    });
                    move.monmap.Add(new JsonMonMap
                    {
                        MonID = monsterModel.Id.ToString(),
                        MonMapID = areaMonster.MonMapId.ToString(),
                        strFrame = areaMonster.Frame,
                        bRed = 0,
                        intRSS = -1
                    });
                    move.mondef.Add(new JsonMonsterDefinition
                    {
                        MonID = monsterModel.Id.ToString(),
                        intHP = monsterModel.Heath,
                        intHPMax = monsterModel.Heath,
                        intMP = monsterModel.Mana,
                        intMPMax = monsterModel.Mana,
                        intLevel = monsterModel.Level,
                        sRace = monsterModel.Race,
                        strBehave = "walk",
                        strElement = monsterModel.Element,
                        strLinkage = monsterModel.Linkage,
                        strMonFileName = monsterModel.File,
                        strMonName = monsterModel.Name
                    });
                }
            }

            NetworkHelper.SendResponse(new JsonPacket(-1, move), user);
            NetworkHelper.SendResponse(new[] {"xt", "server", "-1", "You joined to \"" + Room.Name + "\"!"}, user);
            var uotls = new JsonUotls
            {
                unm = user.Name,
                o = new JsonUoBranch
                {
                    entType = "p",
                    strFrame = user.RoomUser.Frame,
                    strPad = user.RoomUser.Pad,
                    strUsername = user.Character.Name,
                    uoName = user.Name,
                    entID = user.Id,
                    intHP = user.UserState.Health,
                    intHPMax = user.UserState.MaxHealth,
                    intMP = user.UserState.Mana,
                    intMPMax = user.UserState.MaxMana,
                    intLevel = user.Character.Level,
                    intState = user.UserState.State,
                    tx = user.RoomUser.Tx,
                    ty = user.RoomUser.Ty,
                    showCloak = Preference.Is(Indent.Preference.Cloak, user.Character.Settings),
                    showHelm = Preference.Is(Indent.Preference.Helm, user.Character.Settings),
                    afk = user.RoomUser.Afk
                }
            };
            NetworkHelper.SendResponseToOthers(new JsonPacket(Room.Id, uotls), user.RoomUser);
        }

        public bool Remove(RoomUser roomUser)
        {
            if (!LocalUsers.TryRemove(roomUser.UserId, out roomUser)) return false;
            NetworkHelper.SendResponse(
                new XElement("msg", new XAttribute("t", "sys"),
                        new XElement("body", new XAttribute("action", "userGone"), new XAttribute("r", Room.Id),
                            new XElement("user", new XAttribute("id", roomUser.UserId), new XAttribute("m", 0),
                                new XElement("n", new XCData(roomUser.Name)), new XElement("vars", ""))))
                    .ToString(SaveOptions.DisableFormatting), this);
            NetworkHelper.SendResponse(
                new[] {"xt", "exitArea", Room.Id.ToString(), roomUser.UserId.ToString(), roomUser.Name}, this);
            if (Users.Count <= 0) RoomsManager.Remove(Room.Name);
            return true;
        }
    }
}