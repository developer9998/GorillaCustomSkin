using System;
using System.Collections.Generic;
using GorillaCustomSkin.Models;
using GorillaCustomSkin.Tools;
using GorillaTag;
using UnityEngine;

namespace GorillaCustomSkin.Behaviours
{
    [RequireComponent(typeof(VRRig))]
    public class SkinRig : MonoBehaviour
    {
        public VRRig ControllingRig;

        // custom skin

        public event Action<ISkinAsset> OnSkinLoaded;
        public event Action OnSkinUnloaded;

        public bool CustomSkinLoaded = false;

        public ISkinAsset CustomSkin;

        public Dictionary<ISkinAsset, GorillaSkinToggle> SkinToggle = [];

        public void Start()
        {
            ControllingRig ??= GetComponentInParent<VRRig>();
        }

        public void LoadSkin(ISkinAsset skin)
        {
            if (CustomSkinLoaded)
            {
                UnloadSkin();
            }

            Logging.Info($"LoadSkin {skin.Descriptor.Name} for {ControllingRig.Creator.NickName}");

            CustomSkinLoaded = true;
            CustomSkin = skin;

            if (!SkinToggle.TryGetValue(CustomSkin, out var toggle))
            {
                GameObject toggleObject = new($"{skin.Descriptor.Name} GorillaSkinToggle");
                toggleObject.SetActive(false);
                toggleObject.transform.SetParent(ControllingRig.transform, false);

                toggle = toggleObject.AddComponent<GorillaSkinToggle>();
                if (skin.ColouringRule.colorMaterials != 0)
                    toggle.coloringRules = [skin.ColouringRule];
                else
                    toggle.coloringRules = [];
                toggle._skin = skin.Skin;
                (toggle as ISpawnable).OnSpawn(ControllingRig);
                toggle._rig = ControllingRig;
                SkinToggle.Add(skin, toggle);
            }

            toggle.gameObject.SetActive(true);

            OnSkinLoaded?.Invoke(skin);
        }

        public void UnloadSkin()
        {
            if (!CustomSkinLoaded)
                return;

            Logging.Info($"UnloadSkin {CustomSkin.Descriptor.Name} for {ControllingRig.Creator.NickName}");

            CustomSkinLoaded = false;

            if (SkinToggle.TryGetValue(CustomSkin, out var toggle))
            {
                toggle.gameObject.SetActive(false);
            }

            CustomSkin = null;

            OnSkinUnloaded?.Invoke();
        }
    }
}
