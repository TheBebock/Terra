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
        protected override bool CanBeRemoved => !_durationTimer.IsRunning && !IsInfinite;

        private StopwatchTimer _durationTimer;
        private CountdownTimer _tickTimer;

        private bool IsInfinite => Mathf.Approximately(_durationTimer.GetTime(), Constants.StatusInfiniteDuration);

        public float CurrentTime => _durationTimer.GetTime();
        public float Progress => _durationTimer.Progress;

        private float GetTimePerTick()
        {
            float duration = Data.statusDuration;
            // Status is infinite
            if (Mathf.Approximately(duration, Constants.StatusInfiniteDuration))
            {
                return (float)100 / Data.amountOfTicksPerSecond;
            }
            // Status time can vary and be in decimals, so it needs to be computed
            float totalTicks = Mathf.Round(Data.statusDuration * Data.amountOfTicksPerSecond);
            return duration / totalTicks;
        }
        
        protected override void OnApply()
        {
            _durationTimer = new StopwatchTimer();
            _tickTimer = new CountdownTimer(GetTimePerTick());
            
            _tickTimer.Start();
            _tickTimer.OnTimerStop += OnStatusTick;
            
            // If status is infinite, do not start the timer and force set timer time
            if (Mathf.Approximately(Data.statusDuration, Constants.StatusInfiniteDuration))
            {
                _durationTimer.ForceSetTime(Constants.StatusInfiniteDuration);
                return;
            }
            
            _durationTimer.Start();
        }

        protected override void OnUpdate()
        {
            if(IsInfinite) return;
            
            _tickTimer.Tick(Time.deltaTime);
            _durationTimer.Tick(Time.deltaTime);

            // Check for end of status duration
            if (_durationTimer.GetTime() >= Data.statusDuration)
            {
                _durationTimer.Stop();
                return;
            }

            if (!_tickTimer.IsFinished) return;

            // Restart timer for another status tick
            _tickTimer.Restart();
        }

        protected override void OnReset()
        {
            _durationTimer.Reset();
        }

        protected abstract void OnStatusTick();

        protected override void OnRemove()
        {
            _tickTimer.OnTimerStop -= OnStatusTick;
        }
    }
}