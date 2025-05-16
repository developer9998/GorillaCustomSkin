using System;
using System.IO;
using System.Linq;
using GorillaCustomSkin.Behaviours.Networking;
using GorillaCustomSkin.Models;
using GorillaCustomSkin.Tools;
using Loader = GorillaCustomSkin.Models.SkinLoader;
using Rig = GorillaCustomSkin.Behaviours.SkinRig;

namespace GorillaCustomSkin.Behaviours
{
    public class Main : Singleton<Main>
    {
        public Loader Loader;

        public Rig LocalRig;

        public bool IsLoaded;

        public override void Initialize()
        {
            LocalRig = GorillaTagger.Instance.offlineVRRig.AddComponent<Rig>();
            LocalRig.ControllingRig = GorillaTagger.Instance.offlineVRRig;
            LocalRig.OnSkinLoaded += SendModel;
            LocalRig.OnSkinUnloaded += RetractModel;

            Loader = new(Path.GetDirectoryName(typeof(Main).Assembly.Location));
            Loader.OnSkinsLoaded(OnSkinsLoaded);
        }

        public void OnSkinsLoaded()
        {
            IsLoaded = true;

            Configuration.Initialize();

            if (File.Exists(Configuration.CurrentSkin.Value) && Loader.Skins.FirstOrDefault(skin => skin.FilePath == Configuration.CurrentSkin.Value) is var skin)
            {
                LocalRig.LoadSkin(skin);
            }

            // var skins = Loader.Skins;
            // LocalRig.LoadSkin(skins.First());
        }

        private void SendModel(ISkinAsset skin)
        {
            string networkString = string.Concat(skin.Descriptor.Name, skin.Descriptor.Author);
            NetworkHandler.Instance.SetProperty("CustomSkin", new string(Array.FindAll(networkString.ToCharArray(), Utils.IsASCIILetterOrDigit)));

            Configuration.CurrentSkin.Value = skin.FilePath;
        }

        private void RetractModel()
        {
            NetworkHandler.Instance.SetProperty("CustomSkin", string.Empty);

            Configuration.CurrentSkin.Value = string.Empty;
        }
    }
}
