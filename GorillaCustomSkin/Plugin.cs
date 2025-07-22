using BepInEx;
using BepInEx.Logging;
using GorillaCustomSkin.Behaviours;
using GorillaCustomSkin.Behaviours.Networking;
using HarmonyLib;
using UnityEngine;

namespace GorillaCustomSkin
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    [BepInDependency("dev.gorillainfowatch")]
    public class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger;

        public void Awake()
        {
            Logger = base.Logger;

            Harmony.CreateAndPatchAll(GetType().Assembly, Constants.GUID);
            GorillaTagger.OnPlayerSpawned(() => new GameObject(Constants.Name, typeof(DataManager), typeof(NetworkManager), typeof(CustomSkinManager)));
        }
    }
}
