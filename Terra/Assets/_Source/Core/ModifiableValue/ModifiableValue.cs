using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.ModifiableValue
{
   [Serializable]
   public class ModifiableValue
   {
      public float BaseValua;

      public float Value
      {
         get
         {
            if (IsDirty || BaseValua != LastBaseValue)
            {
               LastBaseValue = BaseValua;
               _value = CalculateFinalValua();
               IsDirty = false;
            }

            return _value;
         }
      }

      protected bool IsDirty = true;
      protected float _value;
      protected float LastBaseValue = float.MinValue;
      protected readonly List<ValueModifier> StatModifiers;
      public readonly ReadOnlyCollection<ValueModifier> StatModifiersReadOnly;

      public ModifiableValue()
      {
         StatModifiers = new List<ValueModifier>();
         StatModifiersReadOnly = StatModifiers.AsReadOnly();
      }

      public ModifiableValue(float baseValue) : this()
      {
         BaseValua = baseValue;
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
         float finalValua = BaseValua;
         float sumPercentAdd = 0;
         for (int i = 0; i < StatModifiers.Count; i++)
         {
            ValueModifier mod = StatModifiers[i];
            if (mod.Type == StatModType.Flat)
            {
               finalValua += mod.Value;
            }
            else if (mod.Type == StatModType.PercentAdd)
            {
               sumPercentAdd += mod.Value;
               if (i + 1 >= StatModifiers.Count || StatModifiers[i + 1].Type != StatModType.PercentAdd)
               {
                  finalValua += sumPercentAdd;
                  sumPercentAdd = 0;
               }
            }
            else if (mod.Type == StatModType.PercentMult)
            {
               finalValua *= 1 + mod.Value;
            }
         }

         return (float)Math.Round(finalValua, 4);
      }
   }

}

