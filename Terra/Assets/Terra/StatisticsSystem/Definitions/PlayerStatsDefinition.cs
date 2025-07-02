using UnityEngine;

namespace Terra.StatisticsSystem.Definitions
{
    [CreateAssetMenu(fileName = "PlayerStats_", menuName = "TheBebocks/Statistics/PlayerStats")]
    public class PlayerStatsDefinition : CharacterStatsDefinition
    {
        public int baseLuck;
        public int baseSwingSpeed;
        public int baseMeleeRange;
        public int baseRangeCooldown;
    }
}


