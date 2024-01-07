using System;
using UnityEngine;

public class Timer
{
    private float duration;

    private Action onCompleted;

    private bool repeat = false;

    private float durationLeft;

    private bool paused = false;

    public Timer(float duration, Action onCompleted, bool repeat = false)
    {
        this.duration = duration;
        this.durationLeft = duration;
        this.onCompleted = onCompleted;
        this.repeat = repeat;

        Game.addTimer(this);
    }

    public void Tick()
    {
        if (this.paused)
        {
            return;
        }

        this.durationLeft -= Time.deltaTime;

        if (this.durationLeft > 0)
        {
            return;
        }

        this.onCompleted.Invoke();

        if (!this.repeat)
        {
            this.Destroy();
            return;
        }

        this.Reset();
    }

    public void Destroy()
    {
        Game.deleteTimer(this);
    }

    public void Pause()
    {
        this.paused = true;
    }

    public void Unpause()
    {
        this.paused = false;
    }

    public void Reset()
    {
        this.durationLeft = this.duration;
    }

    public void setDuration(float duration)
    {
        this.duration = duration;
    }

    public void setDurationLeft(float duration)
    {
        this.durationLeft = duration;
    }
}