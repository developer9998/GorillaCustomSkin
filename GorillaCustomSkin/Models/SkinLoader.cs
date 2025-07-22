using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GorillaCustomSkin.Models
{
    public class SkinLoader
    {
        public string BasePath;

        public List<ISkinAsset> Skins;

        private Action onSkinsLoadedRootCallback;

        public SkinLoader(string path)
        {
            BasePath = path;
            Skins = GetAllSkins();
        }

        public void OnSkinsLoaded(Action action)
        {
            if (Skins != null)
            {
                action?.Invoke();
                return;
            }
            onSkinsLoadedRootCallback = (Action)Delegate.Combine(onSkinsLoadedRootCallback, action);
        }

        public List<ISkinAsset> GetAllSkins()
        {
            List<ISkinAsset> materials = [];

            var modded_material_files = Directory.GetFiles(BasePath, "*.gskin", SearchOption.AllDirectories).ToList();

            materials.AddRange(LoadSkins<ModdedSkin>(modded_material_files));

            Task.Run(async delegate ()
            {
                await Task.Delay(5000);
                ThreadingHelper.Instance.StartSyncInvoke(delegate ()
                {
                    onSkinsLoadedRootCallback?.Invoke();
                });
            });

            return materials;
        }

        public List<T> LoadSkins<T>(List<string> files) where T : ISkinAsset
        {
            List<T> materials = [];

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];

                try
                {
                    var asset = (T)Activator.CreateInstance(typeof(T), file);
                    materials.Add(asset);
                }
                catch (Exception ex)
                {
                    Plugin.Logger.LogFatal($"Error constructing skin: {file}");
                    Plugin.Logger.LogError(ex);
                    File.Move(file, string.Concat(file, ".broken"));
                }
            }

            return materials;
        }
    }
}
