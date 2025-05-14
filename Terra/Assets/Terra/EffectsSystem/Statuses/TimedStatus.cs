using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.Utils;
using UnityEngine;

namespace Terra.EffectsSystem.Statuses
{

    /// <summary>
    ///     Represents a status that is time based.
    /// </summary>
    public abstract class TimedStatus<TStatusData> : StatusEffect<TStatusData>
        where TStatusData : TimedStatusData
    {
        protected override bool CanBeRemoved => !durationTimer.IsRunning && !IsInfinite;

        private StopwatchTimer durationTimer;
        private CountdownTimer tickTimer;

        private bool IsInfinite => Mathf.Approximately(durationTimer.GetTime(), Constants.STATUS_INFINITE_DURATION);

        public float CurrentTime => durationTimer.GetTime();
        public float Progress => durationTimer.Progress;

        private float GetTimePerTick()
        {
            float duration = Data.statusDuration;
            // Status is infinite
            if (Mathf.Approximately(duration, Constants.STATUS_INFINITE_DURATION))
            {
                return (float)100 / Data.amountOfTicksPerSecond;
            }
            // Status time can vary and be in decimals, so it needs to be computed
            float totalTicks = Mathf.Round(Data.statusDuration * Data.amountOfTicksPerSecond);
            return duration / totalTicks;
        }
        
        protected override void OnApply()
        {
            durationTimer = new StopwatchTimer();
            tickTimer = new CountdownTimer(GetTimePerTick());
            
            tickTimer.Start();
            tickTimer.OnTimerStop += OnStatusTick;
            
            // If status is infinite, do not start the timer and force set timer time
            if (Mathf.Approximately(Data.statusDuration, Constants.STATUS_INFINITE_DURATION))
            {
                durationTimer.ForceSetTime(Constants.STATUS_INFINITE_DURATION);
                return;
            }
            
            durationTimer.Start();
        }

        protected override void OnUpdate()
        {
            if(IsInfinite) return;
            
            tickTimer.Tick(Time.deltaTime);
            durationTimer.Tick(Time.deltaTime);

            // Check for end of status duration
            if (durationTimer.GetTime() >= Data.statusDuration)
            {
                durationTimer.Stop();
                return;
            }

            if (!tickTimer.IsFinished) return;

            // Restart timer for another status tick
            tickTimer.Restart();
        }

        protected override void OnReset()
        {
            durationTimer.Reset();
        }

        protected abstract void OnStatusTick();

        protected override void OnRemove()
        {
            tickTimer.OnTimerStop -= OnStatusTick;
        }
    }
}