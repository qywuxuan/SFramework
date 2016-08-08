using UnityEngine;
using System.Collections;

public class SCoroutine
{
    public bool IsRunning { get; private set; }
    public bool IsPaused { get; private set; }
    public bool IsStopped { get; private set; }

    public delegate void FinishedHandler(bool manual);
    public event FinishedHandler Finished;

    private IEnumerator coroutine;

    public SCoroutine(IEnumerator coroutine, bool autoStart = true, params FinishedHandler[] finishedHandlers)
    {
        this.coroutine = coroutine;

        foreach(FinishedHandler finishedHandler in finishedHandlers)
        {
            Finished += finishedHandler;
        }

        if (autoStart)
            Start();
    }

    public void Start()
    {
        if(IsRunning)
        {
            //do nothing
        }
        else
        {
            IsRunning = true;
            IsStopped = false;
            IsPaused = false;
            SCoroutineManager.Instance.StartCoroutine(CallWrapper());
        }
    }

    public void Stop()
    {
        IsRunning = false;
        IsStopped = true;
    }

    public void Pause()
    {
        IsPaused = true;
    }

    public void Continue()
    {
        IsPaused = false;
    }

    void TaskFinished(bool manual)
    {
        if (Finished != null)
            Finished(manual);
    }

    IEnumerator CallWrapper()
    {
        yield return null;
        while (IsRunning)
        {
            if (IsPaused)
                yield return null;
            else
            {
                if (coroutine != null && coroutine.MoveNext())
                {
                    yield return coroutine.Current;
                }
                else
                {
                    IsRunning = false;
                }
            }
        }

        TaskFinished(IsStopped);
    }
}
