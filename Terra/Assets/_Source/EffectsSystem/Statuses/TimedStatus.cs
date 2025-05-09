using System;
using System.Collections;
using System.Collections.Generic;
using _Source.AI.Enemy;
using Terra.EffectsSystem.Abstracts;
using Terra.Utils;
using UnityEngine;

public abstract class TimedStatus<TStatusData> : StatusEffect<TStatusData>
    where TStatusData : TimedStatusData
{
    protected override bool CanBeRemoved => durationTimer.IsRunning;

    private StopwatchTimer durationTimer;
    private CountdownTimer tickTimer;

    public float CurrentTime => durationTimer.GetTime();
    public float Progress => durationTimer.Progress;
    private float TimePerTick => Data.statusDuration / Data.amountOfTicks;
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

    protected abstract void OnStatusTick();

    protected override void OnRemove()
    {
        tickTimer.OnTimerStop -= OnStatusTick;
    }
}
