using Assets.Scripts.Types;

namespace Assets.Scripts.Controller.Collectible
{
    public class ValueCollectible : CollectibleItem
    {
        public ValueCollectibleTypes Type { get;set; }
        public float Value { get; set; }
    }
}