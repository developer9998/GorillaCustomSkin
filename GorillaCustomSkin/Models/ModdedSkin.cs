using GorillaCustomSkin.Behaviours;
using System;
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
                Plugin.Logger.LogInfo($"Loading ModdedSkin: {path}");

                FilePath = path;

                var assetBundle = AssetBundle.LoadFromFile(path);

                GameObject template = assetBundle.LoadAsset<GameObject>("CustomSkinAsset");

                if (template.TryGetComponent(out SkinDescriptor descriptor))
                {
                    Plugin.Logger.LogMessage("Got descriptor component");

                    Descriptor = descriptor;

                    var defaultSkin = GorillaTagger.Instance.offlineVRRig.defaultSkin;

                    Skin = ScriptableObject.CreateInstance<GorillaSkin>();
                    Skin._bodyMesh = defaultSkin._bodyMesh;

                    Plugin.Logger.LogMessage("Created GorillaSkin");

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

                    // TODO: re-implement face material

                    /*
                    if (Descriptor.FaceMaterial)
                    {
                        skinMaterials |= GorillaSkinMaterials.Face;
                        Skin._faceMaterial = descriptor.FaceMaterial;
                    }
                    else
                    {
                        Skin._faceMaterial = defaultSkin._faceMaterial;
                    }
                    */

                    Plugin.Logger.LogMessage("Assigned skin materials");

                    if (Descriptor.CustomColours)
                    {
                        var colouringRule = new GorillaSkinToggle.ColoringRule
                        {
                            colorMaterials = skinMaterials
                        };
                        ColouringRule = colouringRule;

                        Plugin.Logger.LogMessage("Defined colouring rule (custom colours)");
                    }
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogFatal($"Error loading skin: {path}");
                Plugin.Logger.LogError(ex);
            }
        }
    }
}
