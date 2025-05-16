using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GorillaCustomSkin.Tools;

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

            onSkinsLoadedRootCallback?.Invoke();

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
                    Logging.Fatal($"Error constructing skin: {file}");
                    Logging.Error(ex);
                    File.Move(file, string.Concat(file, ".broken"));
                }
            }

            return materials;
        }
    }
}
