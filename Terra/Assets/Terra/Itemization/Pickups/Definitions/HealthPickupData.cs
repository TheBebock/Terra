using UnityEngine;

namespace Terra.Itemization.Pickups.Definitions
{
    [CreateAssetMenu(fileName = "HealthPickupData", menuName = "TheBebocks/Pickups/HealthPickupData")]
    public class HealthPickupData : PickupData
    {
        [Tooltip("In percentages")][Range(1,100)]public int maxHealthHealAmount = 10;
    }
}