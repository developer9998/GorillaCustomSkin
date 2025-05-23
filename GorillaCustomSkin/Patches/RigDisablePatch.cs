﻿using GorillaCustomSkin.Behaviours.Networking;
using HarmonyLib;

namespace GorillaCustomSkin.Patches
{
    [HarmonyPatch(typeof(RigContainer), "OnDisable")]
    public class RigDisablePatch
    {
        [HarmonyWrapSafe]
        public static void Postfix(RigContainer __instance)
        {
            if (__instance.TryGetComponent(out NetworkedPlayer networked_player))
            {
                UnityEngine.Object.Destroy(networked_player);
            }
        }
    }
}
