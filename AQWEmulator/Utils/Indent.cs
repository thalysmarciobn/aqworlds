namespace AQWEmulator.Utils
{
    public static class Indent
    {
        public const string Key = "N7B5W8W1Y5B1R5VWVZ~";

        public static class Core
        {
            public const string IntHpPerEnd = "intHPperEND";
            public const string IntHpPerWis = "intMPperWIS";
            public const string LevelMax = "intLevelMax";
            public const string LevelCap = "intLevelCap";
            public const string GstBase = "GstBase";
            public const string GstGoal = "GstGoal";
            public const string StatsExponent = "statsExponent";
            public const string PcMpBase1 = "PCmpBase1";
            public const string PcMpBase100 = "PCmpBase1";
            public const string CurveExponent = "curveExponent";
            public const string PcHpGoal1 = "PChpGoal1";
            public const string PcHpGoal100 = "PChpGoal100";
            public const string PcHpBase1 = "PChpBase1";
            public const string PcHpBase100 = "PChpBase100";
            public const string PcHpDelta = "PChpDelta";
            public const string PcStBase = "PCstBase";
            public const string PcStGoal = "PCstGoal";
            public const string BaseDodge = "baseDodge";
            public const string BaseCrit = "baseCrit";
            public const string BaseMiss = "baseMiss";
            public const string BaseParry = "baseParry";
            public const string BaseBlock = "baseBlock";
            public const string BaseHit = "baseHit";
            public const string BaseHaste = "baseHaste";
            public const string BaseCritValue = "baseCritValue";
            public const string BaseBlockValue = "baseBlockValue";
            public const string BaseResistValue = "baseResistValue";
            public const string BaseEventValue = "baseEventValue";
        }

        public static class State
        {
            public const int IntDead = 0;
            public const int IntNeutral = 1;
            public const int IntCombat = 2;
        }

        public static class Combat
        {
            public const string None = "none";
            public const string Critical = "crit";
            public const string Hit = "hit";
            public const string Miss = "miss";
            public const string Dodge = "dodge";
        }

        public static class Preference
        {
            public const string Animation = "bWAnim";
            public const string Cloak = "bCloak";
            public const string Duel = "bDuel";
            public const string Fbshare = "bFBShare";
            public const string Friend = "bFriend";
            public const string Goto = "bGoto";
            public const string Guild = "bGuild";
            public const string Helm = "bHelm";
            public const string Music = "bMusicOn";
            public const string Party = "bParty";
            public const string Pet = "bPet";
            public const string Sound = "bSoundOn";
            public const string Tooltips = "bTT";
            public const string Whisper = "bWhisper";
        }

        public static class Message
        {
            public const string DuelOff = "Ignoring duel invites.";
            public const string DuelOn = "Accepting duel invites.";
            public const string FriendOff = "Ignoring Friend requests.";
            public const string FriendOn = "Accepting Friend requests.";
            public const string GotoOff = "Blocking goto requests.";
            public const string GotoOn = "Accepting goto requests.";
            public const string GuildOff = "Ignoring guild invites.";
            public const string GuildOn = "Accepting guild invites.";
            public const string PartyOff = "Ignoring party invites.";
            public const string PartyOn = "Accepting party invites.";

            public const string TooltipsOff = "Ability ToolTips will not show on mouseover during combat.";

            public const string TooltipsOn = "Ability ToolTips will always show on mouseover.";
            public const string WhisperOff = "Ignoring PMs.";
            public const string WhisperOn = "Accepting PMs.";
        }
    }
}