using BepInEx;
using GorillaCustomSkin.Behaviours;
using GorillaInfoWatch.Attributes;
using GorillaInfoWatch.Models;

namespace GorillaCustomSkin.Models
{
    [WatchCustomPage]
    internal class CustomSkinItemScreen : WatchScreen
    {
        public override string Title => Constants.Name;

        public override string Description => Skin is null ? string.Empty : Skin.FilePath.Replace(Paths.PluginPath, string.Empty);

        internal static ISkinAsset Skin;

        public override ScreenContent GetContent()
        {
            LineBuilder lines = new();

            if (Skin == null)
            {
                SetScreen<CustomSkinSelectionScreen>();
                return lines;
            }

            lines.AddLine($"Name: {Skin.Descriptor.Name}");

            lines.AddLine($"Author: {Skin.Descriptor.Author}");

            lines.AddLine($"Description: {(string.IsNullOrEmpty(Skin.Descriptor.Description) || string.IsNullOrWhiteSpace(Skin.Descriptor.Description) ? "<color=red>N/A</color>" : Skin.Descriptor.Description)}");

            lines.AddLine($"{(Main.Instance.LocalRig.CustomSkin == Skin ? "Remove" : "Equip")} Skin:", new WidgetButton(WidgetButton.EButtonType.Switch, Main.Instance.LocalRig.CustomSkin == Skin, EquipSkin));

            return lines;
        }

        public void EquipSkin(bool isButtonPressed, object[] args)
        {
            if (Main.Instance.LocalRig.CustomSkin != Skin)
            {
                Main.Instance.LocalRig.LoadSkin(Skin);
            }
            else
            {
                Main.Instance.LocalRig.UnloadSkin();
            }

            SetText();
        }
    }
}
