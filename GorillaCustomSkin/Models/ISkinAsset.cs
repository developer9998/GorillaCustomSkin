using GorillaCustomSkin.Behaviours;

namespace GorillaCustomSkin.Models
{
    public interface ISkinAsset
    {
        public string FilePath { get; }
        public SkinDescriptor Descriptor { get; }
        public GorillaSkin Skin { get; }
        public GorillaSkinToggle.ColoringRule ColouringRule { get; }
    }
}
