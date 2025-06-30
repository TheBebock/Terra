using System;

namespace Terra.Utils {
    public abstract class Timer {
        protected float initialTime;
        protected float Time { get; set; }
        public bool IsRunning { get; protected set; }
        
        public float Progress => Time / initialTime;
        
        public event Action OnTimerStart = delegate { };
        public event Action OnTimerStop = delegate { };

        protected Timer(float value) {
            initialTime = value;
            IsRunning = false;
        }

        public void Start() {
            Time = initialTime;
            if (!IsRunning) {
                IsRunning = true;
                OnTimerStart?.Invoke();
            }
        }

        public void Stop() {
            if (IsRunning) {
                IsRunning = false;
                OnTimerStop?.Invoke();
            }
        }
        
        public void ForceSetTime(float time) => Time = time;
        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;
        
        public abstract void Tick(float deltaTime);
    }
    
    public class CountdownTimer : Timer {
        public CountdownTimer(float value) : base(value) { }

        public override void Tick(float deltaTime) {
            if (IsRunning && Time > 0) {
                Time -= deltaTime;
            }
            
            if (IsRunning && Time <= 0) {
                Stop();
            }
        }
        
        public bool IsFinished => Time <= 0;

        /// <summary>
        /// Resets time back to its initial value and starts the timer
        /// </summary>
        public void Restart()
        {
            ResetTime();
            Start();
        }
        
        /// <summary>
        /// Resets time to given value and starts the timer
        /// </summary>
        public void Restart(float newTime)
        {
            ResetTime(newTime);
            Start();
        }

        /// <summary>
        /// Resets time back to its initial value
        /// </summary>
        public void ResetTime() => Time = initialTime;
        
        /// <summary>
        /// Resets time to given value
        /// </summary>
        public void ResetTime(float newTime) {
            initialTime = newTime;
            ResetTime();
        }
    }
    
    public class StopwatchTimer : Timer {
        public StopwatchTimer() : base(0) { }

        public override void Tick(float deltaTime) {
            if (IsRunning) {
                Time += deltaTime;
            }
        }
        
        public void Reset() => Time = 0;
        
        public float GetTime() => Time;
    }
}