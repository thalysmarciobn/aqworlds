using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using AQWEmulator.Combat;
using AQWEmulator.Database.Models;
using AQWEmulator.World.Core;
using AQWEmulator.Utils.AQW;
using AQWEmulator.World.Users;
using JSON.Model;

namespace AQWEmulator.World
{
    public class Server
    {
        private static Server _instance;
        public static Server Instance => _instance ?? (_instance = new Server());

        public readonly ConcurrentDictionary<string, string> Settings = new ConcurrentDictionary<string, string>();
        public readonly StringBuilder SettingsBuilder = new StringBuilder();

        public void Cache()
        {
            foreach (var setting in ServerCoreValues.FlashSettings())
            {
                if (SettingsBuilder.Length > 0) SettingsBuilder.Append(",");
                SettingsBuilder.Append(setting.Key);
                SettingsBuilder.Append("=");
                SettingsBuilder.Append(setting.Value);
                Settings.TryAdd(setting.Key, setting.Value);
            }
        }

        private Dictionary<string, int> GetItemStats(EnhancementModel enhancement, PatternModel pattern,
            string equipment)
        {
            return ItemStats.Get(
                Math.Round(ServerCoreValues.GetIBudget(enhancement.Level, enhancement.Rarity) *
                           Stats.GetRatiosBySlot(equipment)), new Dictionary<string, int>
                {
                    {"END", pattern.Endurance},
                    {"STR", pattern.Strength},
                    {"INT", pattern.Intelligence},
                    {"DEX", pattern.Dexterity},
                    {"WIS", pattern.Wisdom},
                    {"LCK", pattern.Luck}
                });
        }

        public JsonItem GetItemJson(ItemModel item, EnhancementModel enhancement = null)
        {
            var json = new JsonItem
            {
                ItemID = item.Id,
                bCoins = item.Coins,
                bHouse = item.Equipment.Equals("ho") || item.Equipment.Equals("hi") ? 1 : 0,
                bPTR = 0,
                bStaff = item.Staff,
                bTemp = item.Temporary,
                bUpg = item.Upgrade,
                iCost = item.Cost,
                iDPS = item.Dps,
                iLvl = item.Level,
                iQSindex = item.QuestStringIndex,
                iQSvalue = item.QuestStringValue,
                iRng = item.Range,
                iRty = item.Rarity,
                iStk = item.Stack,
                sDesc = item.Description,
                sES = item.Equipment,
                sElmt = item.Element,
                sIcon = item.Icon,
                sLink = item.Link,
                sMeta = item.Meta,
                sName = item.Name,
                sReqQuests = item.ReqQuests,
                sType = item.Type
            };
            if (enhancement != null)
                if (item.Type.Equals("Enhancement"))
                {
                    json.PatternID = enhancement.PatternId;
                    json.iDPS = enhancement.Dps;
                    json.iLvl = enhancement.Level;
                    json.iRty = enhancement.Rarity;
                    json.EnhID = 0;
                }
                else
                {
                    json.EnhID = enhancement.Id;
                    json.EnhLvl = enhancement.Level;
                    json.EnhPatternID = enhancement.PatternId;
                    json.EnhRty = enhancement.Rarity;
                    json.EnhRng = item.Range;
                    json.InvEnhPatternID = enhancement.PatternId;
                    json.EnhDPS = enhancement.Dps;
                    json.sFile = item.File;
                }
            else
                json.sFile = item.File;

            return json;
        }

        public void UpdateStats(User user, IEnumerable<CharacterItemModel> items)
        {
            foreach (var userItemModel in items)
            {
                var itemModel = userItemModel.Item;
                if (itemModel == null) continue;
                var enhancementModel = userItemModel.Enhancement;
                var patternModel = enhancementModel?.Pattern;
                if (patternModel == null) continue;
                var equipment = itemModel.Equipment;
                var itemStats = GetItemStats(enhancementModel, patternModel, equipment);
                if (equipment == "ar")
                    foreach (var stat in itemStats)
                        user.UserStats.Armor[stat.Key] = stat.Value;
                else if (equipment == "Weapon")
                    foreach (var stat in itemStats)
                        user.UserStats.Weapon[stat.Key] = stat.Value;
                else if (equipment == "ba")
                    foreach (var stat in itemStats)
                        user.UserStats.Cape[stat.Key] = stat.Value;
                else if (equipment == "he")
                    foreach (var stat in itemStats)
                        user.UserStats.Helm[stat.Key] = stat.Value;
                user.UserStats.Update();
            }
        }
    }
}