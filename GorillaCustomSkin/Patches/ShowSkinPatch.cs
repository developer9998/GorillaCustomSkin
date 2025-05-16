using GorillaCustomSkin.Behaviours;
using HarmonyLib;

namespace GorillaCustomSkin.Patches
{
    [HarmonyPatch(typeof(GorillaSkin), nameof(GorillaSkin.ShowSkin))]
    public class ShowSkinPatch
    {
        [HarmonyWrapSafe]
        public static bool Prefix(VRRig rig, bool useDefaultBodySkin)
        {
            if (useDefaultBodySkin && rig.TryGetComponent(out SkinRig customSkinRig) && customSkinRig.CustomSkinLoaded)
            {
                customSkinRig.LoadSkin(customSkinRig.CustomSkin);
                return false;
            }
            return true;
        }
    }
}
