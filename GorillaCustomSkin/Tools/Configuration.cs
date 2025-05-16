using BepInEx.Configuration;

namespace GorillaCustomSkin.Tools
{
    public class Configuration
    {
        public static ConfigEntry<string> CurrentSkin;

        public static void Initialize()
        {
            Plugin.MainConfig.SaveOnConfigSet = true;

            CurrentSkin = Plugin.MainConfig.Bind(Constants.Name, "Current Skin", string.Empty, "The location of the current custom skin.");
        }
    }
}
