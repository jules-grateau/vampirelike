using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Types
{
    public class StatsUpgrade<T,U>: Upgrade<T> where T : BaseStatsUpgradeSO<U>
    {
        public StatsUpgrade(UpgradeQuality quality, T upgradeSO) : base(quality, upgradeSO)
        {
        }

        public float GetValue()
        {
            return UpgradeSO.GetValue(UpgradeQuality);
        }
    }
}