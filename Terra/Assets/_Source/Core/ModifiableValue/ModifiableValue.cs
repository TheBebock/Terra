using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Core.ModifiableValue
{
   [Serializable]
   public class ModifiableValue
   {
      [SerializeField] private float baseValue;

      public float Value
      {
         get
         {
            if (IsDirty || baseValue != LastBaseValue)
            {
               LastBaseValue = baseValue;
               _value = CalculateFinalValua();
               IsDirty = false;
            }

            return _value;
         }
      }

      protected bool IsDirty = true;
      protected float _value;
      protected float LastBaseValue = float.MinValue;
      [SerializeField] protected List<ValueModifier> StatModifiers;

      public ModifiableValue()
      {
         StatModifiers = new List<ValueModifier>();
      }

      public ModifiableValue(float baseValue) : this()
      {
         baseValue = baseValue;
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

      public virtual bool RemoveAllModifiersFromSource(object source)
      {
         bool didRemove = false;
         for (int i = StatModifiers.Count - 1; i >= 0; i++)
         {
            if (StatModifiers[i].Source == source)
            {
               IsDirty = true;
               didRemove = true;
               StatModifiers.RemoveAt(i);
            }
         }

         return didRemove;
      }

      protected virtual float CalculateFinalValua()
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

