using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GorillaCustomSkin.Behaviours;
using GorillaCustomSkin.Behaviours.Networking;
using HarmonyLib;
using UnityEngine;

namespace GorillaCustomSkin
{
    [BepInDependency("dev.gorillainfowatch")]
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource MainLogSource;

        public static ConfigFile MainConfig;

        public void Awake()
        {
            MainLogSource = Logger;
            MainConfig = Config;

            Harmony.CreateAndPatchAll(GetType().Assembly, Constants.GUID);
            GorillaTagger.OnPlayerSpawned(() => new GameObject(Constants.Name, typeof(Main), typeof(NetworkHandler)));
        }
    }
}
