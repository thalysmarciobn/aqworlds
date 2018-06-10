using AQWEmulator.Attributes;
using AQWEmulator.Database;
using AQWEmulator.Database.Models;
using AQWEmulator.Helper;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;

namespace AQWEmulator.Network.Packet.Events
{
    
    [PacketIn("genderSwap")]
    public class GenderSwap : IPacketHandler
    {
        private const int Cost = 5000;
        
        public void Dispatch(User user, int room, string[] parameters)
        {
            var charcater = user.Character;
            if (charcater.Coins < Cost)
            {
                NetworkHelper.SendResponse(new[] {"xt", "warning", "-1", "You don\'t have enough coins!"}, user);
                return;
            }
            if (charcater.Gender.Equals("M"))
            {
                ChangeGender(user, room, "F", 15);
            }
            else
            {
                ChangeGender(user, room, "M", 1);
            }
        }

        private static void ChangeGender(User user, int room, string gender, int hairId)
        {
            using (var session = GameFactory.Session.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var hairModel = session.QueryOver<HairModel>().Where(x => x.Id == hairId).SingleOrDefault();
                    if (hairModel == null) return;
                    var characterModel = user.Character;
                    characterModel.HairId = hairId;
                    characterModel.Coins -= 5000;
                    characterModel.Gender = gender;
                    characterModel.Hair = hairModel;
                    session.Update(characterModel);
                    transaction.Commit();
                    var genderSwap = new JsonGenderSwap()
                    {
                        uid = user.Id,
                        HairID = hairId,
                        bitSuccess = 1,
                        intCoins = Cost,
                        gender = gender,
                        strHairName = hairModel.Name,
                        strHairFilename = hairModel.File
                    };
                    NetworkHelper.SendResponse(new JsonPacket(room, genderSwap), user.RoomUser);
                }
            }
        }
    }
}