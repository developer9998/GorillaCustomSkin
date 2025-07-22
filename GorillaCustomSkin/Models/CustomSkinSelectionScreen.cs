using System.Linq;
using GorillaCustomSkin.Behaviours;
using GorillaInfoWatch.Attributes;
using GorillaInfoWatch.Models;
using GorillaInfoWatch.Models.Widgets;

[assembly: InfoWatchCompatible]

namespace GorillaCustomSkin.Models
{
    [ShowOnHomeScreen]
    internal class CustomSkinSelectionScreen : InfoWatchScreen
    {
        public override string Title => Constants.Name;

        public override string Description => !CustomSkinManager.Instance.IsLoaded ? "GorillaCustomSkin is actively loading, please wait, then refresh" : $"{CustomSkinManager.Instance.Loader.Skins.Count} skins loaded by GorillaCustomSkin";

        public override ScreenContent GetContent()
        {
            
            LineBuilder lines = new();

            if (CustomSkinManager.Instance.IsLoaded)
            {
                foreach (var skin in CustomSkinManager.Instance.Loader.Skins)
                {
                    lines.Add(skin.Descriptor.Name, new Widget_PushButton(ViewSkin, skin));
                }
            }

            return lines;
        }

        public void ViewSkin(object[] parameters)
        {
            if (parameters.ElementAtOrDefault(0) is ISkinAsset skin)
            {
                CustomSkinItemScreen.Skin = skin;
                SetScreen<CustomSkinItemScreen>();
            }
        }
    }
}
