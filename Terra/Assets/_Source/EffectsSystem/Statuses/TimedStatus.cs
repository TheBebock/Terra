using Terra.EffectsSystem.Abstracts;
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
        protected override bool CanBeRemoved => !durationTimer.IsRunning;

        private StopwatchTimer durationTimer;
        private CountdownTimer tickTimer;

        public float CurrentTime => durationTimer.GetTime();
        public float Progress => durationTimer.Progress;
        private float TotalTicks => Mathf.Round(Data.statusDuration * Data.amountOfTicksPerSecond);
        private float TimePerTick => Data.statusDuration / TotalTicks;

        protected override void OnApply()
        {
            durationTimer = new StopwatchTimer();
            durationTimer.Start();

            tickTimer = new CountdownTimer(TimePerTick);
            tickTimer.Start();
            tickTimer.OnTimerStop += OnStatusTick;
        }

        protected override void OnUpdate()
        {
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