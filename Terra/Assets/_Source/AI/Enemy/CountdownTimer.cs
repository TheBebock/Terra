public class CountdownTimer
{
    private float duration;
    private float timeRemaining;

    public bool IsRunning => timeRemaining > 0f;
    public bool IsFinished => timeRemaining <= 0f;

    public CountdownTimer(float duration)
    {
        this.duration = duration;
        this.timeRemaining = 0f;
    }

    public void Start()
    {
        timeRemaining = duration;
    }

    public void Reset()
    {
        Start();
    }

    public void Tick(float deltaTime)
    {
        if (IsRunning)
            timeRemaining -= deltaTime;
    }
}