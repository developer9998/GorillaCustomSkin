using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GorillaCustomSkin.Models;
using Photon.Realtime;
using UnityEngine;

namespace GorillaCustomSkin.Behaviours.Networking
{
    // https://github.com/developer9998/GorillaInfoWatch/blob/fbfcf043668cd4e90963836a5aecdbbbc56e8e6a/GorillaInfoWatch/Behaviours/Networking/NetworkedPlayer.cs
    [RequireComponent(typeof(RigContainer)), DisallowMultipleComponent]
    public class NetworkedPlayer : MonoBehaviour
    {
        public VRRig Rig;
        public NetPlayer Owner;

        public bool HasCustomSkin;

        private SkinRig custom_skin_rig;

        public async Task Start()
        {
            if (!Rig.TryGetComponent(out custom_skin_rig))
            {
                custom_skin_rig = Rig.gameObject.AddComponent<SkinRig>();
                custom_skin_rig.Rig = Rig;
            }

            NetworkManager.Instance.OnPlayerPropertyChanged += OnPlayerPropertyChanged;

            await Task.Delay(300);

            Player player = Owner.GetPlayerRef();
            NetworkManager.Instance.OnPlayerPropertiesUpdate(player, player.CustomProperties);
        }

        public void OnDestroy()
        {
            NetworkManager.Instance.OnPlayerPropertyChanged -= OnPlayerPropertyChanged;

            if (HasCustomSkin)
            {
                HasCustomSkin = false;
                custom_skin_rig.UnloadSkin();
            }
        }

        public void OnPlayerPropertyChanged(NetPlayer player, Dictionary<string, object> properties)
        {
            if (player == Owner)
            {
                Plugin.Logger.LogInfo($"{player.NickName} got properties: {string.Join(", ", properties.Select(prop => $"[{prop.Key}: {prop.Value}]"))}");

                if (properties.TryGetValue("CustomSkin", out object custom_skin_property) && custom_skin_property is string custom_skin_name)
                {
                    ISkinAsset skin = Singleton<CustomSkinManager>.Instance.Loader.Skins.Find(skin => new string(Array.FindAll(string.Concat(skin.Descriptor.Name, skin.Descriptor.Author).ToCharArray(), Utils.IsASCIILetterOrDigit)) == custom_skin_name);
                    if (skin != null) custom_skin_rig.LoadSkin(skin);
                    else custom_skin_rig.UnloadSkin();
                }
            }
        }
    }
}
