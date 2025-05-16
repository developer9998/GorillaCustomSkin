using System;
using GorillaCustomSkin.Behaviours;
using GorillaCustomSkin.Tools;
using UnityEngine;

namespace GorillaCustomSkin.Models
{
    public class ModdedSkin : ISkinAsset
    {
        public string FilePath { get; private set; }
        public SkinDescriptor Descriptor { get; private set; }
        public GorillaSkin Skin { get; private set; }
        public GorillaSkinToggle.ColoringRule ColouringRule { get; }

        public ModdedSkin(string path)
        {
            try
            {
                Logging.Info($"Loading ModdedSkin: {path}");

                FilePath = path;

                var assetBundle = AssetBundle.LoadFromFile(path);

                GameObject template = assetBundle.LoadAsset<GameObject>("CustomSkinAsset");

                if (template.TryGetComponent(out SkinDescriptor descriptor))
                {
                    Logging.Info("Got descriptor component");

                    Descriptor = descriptor;

                    var defaultSkin = GorillaTagger.Instance.offlineVRRig.defaultSkin;

                    Skin = ScriptableObject.CreateInstance<GorillaSkin>();
                    Skin._bodyMesh = defaultSkin._bodyMesh;

                    Logging.Info("Created GorillaSkin");

                    GorillaSkinMaterials skinMaterials = 0;

                    if (Descriptor.BodyMaterial)
                    {
                        skinMaterials |= GorillaSkinMaterials.Body;
                        Skin._bodyMaterial = descriptor.BodyMaterial;
                    }
                    else
                    {
                        Skin._bodyMaterial = defaultSkin._bodyMaterial;
                    }

                    if (Descriptor.ChestMaterial)
                    {
                        skinMaterials |= GorillaSkinMaterials.Chest;
                        Skin._chestMaterial = descriptor.ChestMaterial;
                    }
                    else
                    {
                        Skin._chestMaterial = defaultSkin._chestMaterial;
                    }

                    if (Descriptor.ScoreboardMaterial)
                    {
                        skinMaterials |= GorillaSkinMaterials.Scoreboard;
                        Skin._scoreboardMaterial = descriptor.ScoreboardMaterial;
                    }
                    else
                    {
                        Skin._scoreboardMaterial = defaultSkin._scoreboardMaterial;
                    }

                    if (Descriptor.FaceMaterial)
                    {
                        skinMaterials |= GorillaSkinMaterials.Face;
                        Skin._faceMaterial = descriptor.FaceMaterial;
                    }
                    else
                    {
                        Skin._faceMaterial = defaultSkin._faceMaterial;
                    }

                    Logging.Info("Assigned skin materials");

                    if (Descriptor.CustomColours)
                    {
                        var colouringRule = new GorillaSkinToggle.ColoringRule
                        {
                            colorMaterials = skinMaterials
                        };
                        ColouringRule = colouringRule;

                        Logging.Info("Defined colouring rule (custom colours)");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Fatal($"Error loading skin: {path}");
                Logging.Error(ex);
            }
        }
    }
}
