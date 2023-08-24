using Assets.Scripts.Controller.Collectible.Soul;

namespace Assets.Scripts.Controller.Collectible
{
    public class XpCollectible : ValueCollectible
    {
        void Start()
        {
            SoulColorController soulColorController = GetComponent<SoulColorController>();
            if (!soulColorController) return;

            soulColorController.Init(Value / 100);
        }
    }
}