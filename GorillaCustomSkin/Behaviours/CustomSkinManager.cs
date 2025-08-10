using GorillaCustomSkin.Behaviours.Networking;
using GorillaCustomSkin.Models;
using System;
using System.IO;
using Loader = GorillaCustomSkin.Models.SkinLoader;
using Rig = GorillaCustomSkin.Behaviours.SkinRig;

namespace GorillaCustomSkin.Behaviours
{
    public class CustomSkinManager : Singleton<CustomSkinManager>
    {
        public Loader Loader;

        public Rig LocalRig;

        public bool IsLoaded;

        public override void Initialize()
        {
            LocalRig = GorillaTagger.Instance.offlineVRRig.AddComponent<Rig>();
            LocalRig.Rig = GorillaTagger.Instance.offlineVRRig;
            LocalRig.OnSkinLoaded += OnLocalSkinLoaded;
            LocalRig.OnSkinUnloaded += OnLocalSkinUnloaded;

            Loader = new(Path.GetDirectoryName(typeof(CustomSkinManager).Assembly.Location));
            Loader.OnSkinsLoaded(OnSkinsLoaded);
        }

        public void OnSkinsLoaded()
        {
            Plugin.Logger.LogMessage("GorillaCustomSkin loaded");
            IsLoaded = true;

            string currentSkin = DataManager.Instance.GetItem("CustomSkin", string.Empty);
            foreach (ISkinAsset skinAsset in Loader.Skins)
            {
                if (skinAsset.FilePath == currentSkin && skinAsset.Descriptor is SkinDescriptor descriptor && descriptor)
                {
                    Plugin.Logger.LogMessage("Loading configured custom skin");
                    Plugin.Logger.LogInfo($"{descriptor.Name} at {skinAsset.FilePath}");
                    LocalRig.LoadSkin(skinAsset);
                    break;
                }
            }

            // var skins = Loader.Skins;
            // LocalRig.LoadSkin(skins.First());
        }

        private void OnLocalSkinLoaded(ISkinAsset skin)
        {
            DataManager.Instance.AddItem("CustomSkin", skin.FilePath);

            try
            {
                string networkString = string.Concat(skin.Descriptor.Name, skin.Descriptor.Author);
                NetworkManager.Instance.SetProperty("CustomSkin", new string(Array.FindAll(networkString.ToCharArray(), Utils.IsASCIILetterOrDigit)));
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogFatal("Could not supply NetworkManager with custom skin");
                Plugin.Logger.LogError(ex);
            }
        }

        private void OnLocalSkinUnloaded()
        {
            DataManager.Instance.AddItem("CustomSkin", string.Empty);

            try
            {
                NetworkManager.Instance.SetProperty("CustomSkin", string.Empty);
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogFatal("Could not supply NetworkManager with custom skin");
                Plugin.Logger.LogError(ex);
            }
        }
    }
}
