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

        public override void OnScreenOpen()
        {
            Build();
        }

        public void Build()
        {
            LineBuilder = new();

            if (Skin == null)
            {
                ShowScreen(typeof(CustomSkinSelectionScreen));
                return;
            }

            LineBuilder.AddLine($"Name: {Skin.Descriptor.Name}");

            LineBuilder.AddLine($"Author: {Skin.Descriptor.Author}");

            LineBuilder.AddLine($"Description: {(string.IsNullOrEmpty(Skin.Descriptor.Description) || string.IsNullOrWhiteSpace(Skin.Descriptor.Description) ? "<color=red>N/A</color>" : Skin.Descriptor.Description)}");

            LineBuilder.AddLine($"{(Main.Instance.LocalRig.CustomSkin == Skin ? "Remove" : "Equip")} Skin:", new WidgetButton(WidgetButton.EButtonType.Switch, Main.Instance.LocalRig.CustomSkin == Skin, EquipSkin));
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

            Build();
            UpdateLines();
        }
    }
}
