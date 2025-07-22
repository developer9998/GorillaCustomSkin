using BepInEx;
using GorillaCustomSkin.Behaviours;
using GorillaInfoWatch.Models;
using GorillaInfoWatch.Models.Widgets;

namespace GorillaCustomSkin.Models
{
    internal class CustomSkinItemScreen : InfoWatchScreen
    {
        public override string Title => Constants.Name;

        public override string Description => Skin is null ? string.Empty : Skin.FilePath.Replace(Paths.PluginPath, string.Empty).TrimStart('/').TrimStart('\\');

        internal static ISkinAsset Skin;

        public override ScreenContent GetContent()
        {
            LineBuilder lines = new();

            if (Skin == null)
            {
                SetScreen<CustomSkinSelectionScreen>();
                return lines;
            }

            lines.Append("Name: ").Append(Skin.Descriptor.Name).AppendLine();

            lines.Append("Author: ").Append(Skin.Descriptor.Author).AppendLine();

            string description = Skin.Descriptor.Description;
            if (!string.IsNullOrEmpty(description) && !string.IsNullOrWhiteSpace(description)) lines.Append("Description: ").Append(description).AppendLine();

            lines.Append(CustomSkinManager.Instance.LocalRig.CustomSkin == Skin ? "Remove" : "Equip").Append(" Skin:").Add(new Widget_Switch(CustomSkinManager.Instance.LocalRig.CustomSkin == Skin, EquipSkin));

            return lines;
        }

        public void EquipSkin(bool isButtonPressed, object[] args)
        {
            if (CustomSkinManager.Instance.LocalRig.CustomSkin != Skin)
            {
                CustomSkinManager.Instance.LocalRig.LoadSkin(Skin);
            }
            else
            {
                CustomSkinManager.Instance.LocalRig.UnloadSkin();
            }

            SetText();
        }
    }
}
