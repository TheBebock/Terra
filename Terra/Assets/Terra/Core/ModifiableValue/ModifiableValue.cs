using System;
using System.Collections.Generic;
using Terra.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Core.ModifiableValue
{
   [Serializable]
   public class ModifiableValue
   {
      [FormerlySerializedAs("baseValue")] [SerializeField] private int _baseValue;
      [FormerlySerializedAs("StatModifiers")] [SerializeField] protected List<ValueModifier> _statModifiers = new();


      protected bool isDirty = true;
      protected int value;
      protected int lastBaseValue = int.MinValue;
      public int Value
      {
         get
         {
            if (isDirty || _baseValue != lastBaseValue)
            {
               lastBaseValue = _baseValue;
               value = CalculateFinalValue();
               isDirty = false;
            }

            return value;
         }
      }

      public ModifiableValue(int baseValue)
      {
         this._baseValue = baseValue;
      }

      public virtual void AddStatModifier(ValueModifier mod)
      {
         isDirty = true;
         _statModifiers.Add(mod);
         _statModifiers.Sort(CompareModifierOrder);
      }

      protected virtual int CompareModifierOrder(ValueModifier a, ValueModifier b)
      {
         if (a.order < b.order)
            return -1;
         else if (a.order > b.order)
            return 1;
         return 0;
      }

      public virtual bool RemoveStatModifier(ValueModifier mod)
      {
         if (_statModifiers.Remove(mod))
         {
            isDirty = true;
            return true;
         }

         return false;
      }

      public virtual bool RemoveAllModifiersFromSource(int sourceID)
      {
         bool didRemove = false;
         for (int i = _statModifiers.Count - 1; i >= 0; i++)
         {
            if (_statModifiers[i].sourceID == sourceID)
            {
               isDirty = true;
               didRemove = true;
               _statModifiers.RemoveAt(i);
            }
         }

         return didRemove;
      }

      protected virtual int CalculateFinalValue()
      {
         float finalValue = _baseValue;
         float sumPercentAdd = 0;
         for (int i = 0; i < _statModifiers.Count; i++)
         {
            ValueModifier mod = _statModifiers[i];
            switch (mod.type)
            {
               case StatModType.Flat:
                  finalValue += mod.value;
                  break;

               case StatModType.PercentMult:
                  sumPercentAdd += mod.value;
                  if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].type != StatModType.PercentMult)
                  {
                     finalValue *= (1 + sumPercentAdd.ToFactor());
                     sumPercentAdd = 0;
                  }
                  break;
            }
         }


         return (int)Math.Round(finalValue, 4);
      }
   }

}

