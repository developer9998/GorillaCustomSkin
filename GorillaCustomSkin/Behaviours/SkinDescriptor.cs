using UnityEngine;

namespace GorillaCustomSkin.Behaviours
{
    [AddComponentMenu("GorillaCustomSkin/SkinDescriptor")]
    public class SkinDescriptor : MonoBehaviour
    {
        [Tooltip("The name of the skin")]
        public string Name = "Skin";

        [Tooltip("The author of the skin")]
        public string Author = "Author";

        [Tooltip("Additional descriptive info of the skin")]
        public string Description = string.Empty;

        [Tooltip("When enabled, a colouring rule will allow for skin materials to recolour to the player's colour")]
        public bool CustomColours;

        [Tooltip("The main (body) material of the skin")]
        public Material BodyMaterial;

        [Tooltip("The chest material of the skin")]
        public Material ChestMaterial;

        [Tooltip("The material used to symbolize the skin on the scoreboard")]
        public Material ScoreboardMaterial;

        [HideInInspector, Tooltip("The face material of the skin (currently goes unused in-game according to Graze)")]
        public Material FaceMaterial;
    }
}
