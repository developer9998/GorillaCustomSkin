using System;
using System.Collections.Generic;
using GorillaCustomSkin.Models;
using GorillaInfoWatch.Extensions;
using GorillaTag;
using UnityEngine;

namespace GorillaCustomSkin.Behaviours
{
    [RequireComponent(typeof(VRRig)), DisallowMultipleComponent]
    public class SkinRig : MonoBehaviour
    {
        public VRRig Rig;

        public event Action<ISkinAsset> OnSkinLoaded;
        public event Action OnSkinUnloaded;

        public bool CustomSkinLoaded = false;
        public ISkinAsset CustomSkin;

        public Dictionary<ISkinAsset, GorillaSkinToggle> SkinToggleCache = [];

        public void Awake()
        {
            Rig = GetComponent<VRRig>();
        }

        public void LoadSkin(ISkinAsset skin)
        {
            if (CustomSkinLoaded)
            {
                UnloadSkin();
            }

            Plugin.Logger.LogInfo($"LoadSkin {skin.Descriptor.Name} for {((!Rig.isLocal && Rig.Creator is NetPlayer player) ? player.GetNameRef().SanitizeName() : "Local Player")}");

            CustomSkinLoaded = true;
            CustomSkin = skin;

            if (!SkinToggleCache.TryGetValue(CustomSkin, out var toggle))
            {
                GameObject toggleObject = new($"GorillaSkinToggle: {skin.Descriptor.Name}");
                toggleObject.SetActive(false);
                toggleObject.transform.SetParent(Rig.transform, false);

                toggle = toggleObject.AddComponent<GorillaSkinToggle>();
                if (skin.ColouringRule.colorMaterials > 0) toggle.coloringRules = [skin.ColouringRule];
                else toggle.coloringRules = [];
                toggle._skin = skin.Skin;
                (toggle as ISpawnable).OnSpawn(Rig);
                toggle._rig = Rig;
                SkinToggleCache.Add(skin, toggle);
            }

            toggle.gameObject.SetActive(true);

            OnSkinLoaded?.Invoke(skin);
        }

        public void UnloadSkin()
        {
            if (!CustomSkinLoaded)
                return;

            Plugin.Logger.LogInfo($"UnloadSkin {CustomSkin.Descriptor.Name} for {((!Rig.isLocal && Rig.Creator is NetPlayer player) ? player.GetNameRef().SanitizeName() : "Local Player")}");

            CustomSkinLoaded = false;

            if (SkinToggleCache.TryGetValue(CustomSkin, out var toggle))
            {
                toggle.gameObject.SetActive(false);
            }

            CustomSkin = null;

            OnSkinUnloaded?.Invoke();
        }
    }
}
