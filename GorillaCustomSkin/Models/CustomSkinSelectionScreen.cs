using System.IO;
using System.Linq;
using GorillaCustomSkin.Behaviours;
using GorillaInfoWatch.Attributes;
using GorillaInfoWatch.Models;

[assembly: WatchCompatibleMod]

namespace GorillaCustomSkin.Models
{
    [WatchCustomPage, DisplayAtHomeScreen]
    internal class CustomSkinSelectionScreen : WatchScreen
    {
        public override string Title => Constants.Name;

        public override string Description => !Main.Instance.IsLoaded ? "GorillaCustomSkin is actively loading, please wait, then refresh" : $"{Main.Instance.Loader.Skins.Count} skins loaded by GorillaCustomSkin";

        public override ScreenContent GetContent()
        {
            LineBuilder lines = new();

            if (Main.Instance.IsLoaded)
            {
                foreach (var skin in Main.Instance.Loader.Skins)
                {
                    lines.AddLine(Path.GetFileName(skin.FilePath), new WidgetButton(ViewSkin, skin));
                }
            }

            return lines;
        }

        public void ViewSkin(bool isButtonPressed, object[] parameters)
        {
            if (parameters.ElementAtOrDefault(0) is ISkinAsset skin)
            {
                CustomSkinItemScreen.Skin = skin;
                SetScreen<CustomSkinItemScreen>();
            }
        }
    }
}
