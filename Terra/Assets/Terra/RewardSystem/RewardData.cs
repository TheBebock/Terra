using UnityEngine;

namespace Terra.RewardSystem
{
    public class RewardData : IReward
    {
        protected string rewardName;
        protected string rewardDescription;
        protected Sprite rewardSpriteIcon;

        public virtual void ApplyReward()
        {
            
        }

        public virtual void GetRandomReward()
        {

        }
    }
}