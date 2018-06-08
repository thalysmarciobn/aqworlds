using System;

namespace Utils.AQW
{
    public static class Preference
    {
        public static bool Is(string pref, int setting)
        {
            if (pref.Equals(Indent.Preference.Cloak))
                return Get(setting, 0) == 0;
            if (pref.Equals(Indent.Preference.Helm))
                return Get(setting, 1) == 0;
            if (pref.Equals(Indent.Preference.Pet))
                return Get(setting, 2) == 0;
            if (pref.Equals(Indent.Preference.Animation))
                return Get(setting, 3) == 0;
            if (pref.Equals(Indent.Preference.Goto))
                return Get(setting, 4) == 0;
            if (pref.Equals(Indent.Preference.Sound))
                return Get(setting, 5) == 0;
            if (pref.Equals(Indent.Preference.Music))
                return Get(setting, 6) == 0;
            if (pref.Equals(Indent.Preference.Friend))
                return Get(setting, 7) == 0;
            if (pref.Equals(Indent.Preference.Party))
                return Get(setting, 8) == 0;
            if (pref.Equals(Indent.Preference.Guild))
                return Get(setting, 9) == 0;
            if (pref.Equals(Indent.Preference.Whisper))
                return Get(setting, 10) == 0;
            if (pref.Equals(Indent.Preference.Tooltips))
                return Get(setting, 11) == 0;
            if (pref.Equals(Indent.Preference.Fbshare))
                return Get(setting, 12) == 0;
            if (pref.Equals(Indent.Preference.Duel))
                return Get(setting, 13) == 0;
            return false;
        }

        public static int Set(string pref, int intValue, bool value)
        {
            var setting = value ? 0 : 1;
            if (pref.Equals(Indent.Preference.Cloak))
                return Update(intValue, 0, setting);
            if (pref.Equals(Indent.Preference.Helm))
                return Update(intValue, 1, setting);
            if (pref.Equals(Indent.Preference.Pet))
                return Update(intValue, 2, setting);
            if (pref.Equals(Indent.Preference.Animation))
                return Update(intValue, 3, setting);
            if (pref.Equals(Indent.Preference.Goto))
                return Update(intValue, 4, setting);
            if (pref.Equals(Indent.Preference.Sound))
                return Update(intValue, 5, setting);
            if (pref.Equals(Indent.Preference.Music))
                return Update(intValue, 6, setting);
            if (pref.Equals(Indent.Preference.Friend))
                return Update(intValue, 7, setting);
            if (pref.Equals(Indent.Preference.Party))
                return Update(intValue, 8, setting);
            if (pref.Equals(Indent.Preference.Guild))
                return Update(intValue, 9, setting);
            if (pref.Equals(Indent.Preference.Whisper))
                return Update(intValue, 10, setting);
            if (pref.Equals(Indent.Preference.Tooltips))
                return Update(intValue, 11, setting);
            if (pref.Equals(Indent.Preference.Fbshare))
                return Update(intValue, 12, setting);
            return pref.Equals(Indent.Preference.Duel) ? Update(intValue, 13, setting) : 0;
        }

        private static int Get(int value, int index)
        {
            return index >= 0 && index <= 31 ? ((value & (int) Math.Pow(2.0D, index)) == 0 ? 0 : 1) : -1;
        }

        private static int Update(int valueToSet, int index, int value)
        {
            switch (value)
            {
                case 0:
                    return valueToSet & ~(int) Math.Pow(2.0D, index);
                case 1:
                    return valueToSet | (int) Math.Pow(2.0D, index);
            }

            return 0;
        }
    }
}