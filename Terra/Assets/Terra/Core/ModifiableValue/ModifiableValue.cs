using System;
using System.Collections.Generic;
using UnityEngine;

namespace Terra.Core.ModifiableValue
{
   [Serializable]
   public class ModifiableValue
   {
      [SerializeField] private float baseValue;
      [SerializeField] protected List<ValueModifier> StatModifiers = new();


      protected bool IsDirty = true;
      protected float _value = 0;
      protected float LastBaseValue = float.MinValue;
      public float Value
      {
         get
         {
            if (IsDirty || baseValue != LastBaseValue)
            {
               LastBaseValue = baseValue;
               _value = CalculateFinalValue();
               IsDirty = false;
            }

            return _value;
         }
      }

      public ModifiableValue(float baseValue)
      {
         this.baseValue = baseValue;
      }

      public virtual void AddStatModifier(ValueModifier mod)
      {
         IsDirty = true;
         StatModifiers.Add(mod);
         StatModifiers.Sort(CompareModifierOrder);
      }

      protected virtual int CompareModifierOrder(ValueModifier a, ValueModifier b)
      {
         if (a.Order < b.Order)
            return -1;
         else if (a.Order > b.Order)
            return 1;
         return 0;
      }

      public virtual bool RemoveStatModifier(ValueModifier mod)
      {
         if (StatModifiers.Remove(mod))
         {
            IsDirty = true;
            return true;
         }

         return false;
      }

      public virtual bool RemoveAllModifiersFromSource(int sourceID)
      {
         bool didRemove = false;
         for (int i = StatModifiers.Count - 1; i >= 0; i++)
         {
            if (StatModifiers[i].SourceID == sourceID)
            {
               IsDirty = true;
               didRemove = true;
               StatModifiers.RemoveAt(i);
            }
         }

         return didRemove;
      }

      protected virtual float CalculateFinalValue()
      {
         float finalValue = baseValue;
         float sumPercentAdd = 0;
         for (int i = 0; i < StatModifiers.Count; i++)
         {
            ValueModifier mod = StatModifiers[i];
            switch (mod.Type)
            {
               case StatModType.Flat:
                  finalValue += mod.Value;
                  break;

               case StatModType.PercentAdd:
                  sumPercentAdd += mod.Value;
                  if (i + 1 >= StatModifiers.Count || StatModifiers[i + 1].Type != StatModType.PercentAdd)
                  {
                     finalValue *= 1 + sumPercentAdd;
                     sumPercentAdd = 0;
                  }
                  break;

               case StatModType.PercentMult:
                  finalValue *= 1 + mod.Value;
                  break;
            }
         }


         return (float)Math.Round(finalValue, 4);
      }
   }

}

