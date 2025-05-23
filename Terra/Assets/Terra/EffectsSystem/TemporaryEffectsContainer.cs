using NUnit.Framework;
using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract.Definitions;

namespace Terra.EffectsSystem
{
    public class TemporaryEffectsContainer:PersistentMonoSingleton<TemporaryEffectsContainer>
    {
        public List<ActionEffectData> actionEffectDatas = new();
        public List<StatusEffectData> statusEffectDatas = new();
    }
}