using UnityEngine;
using System.Collections;

public class SCoroutineLauncher : MonoBehaviour
{
	void Start ()
    {
        var scoroutine =  new SCoroutine(TimeCounter());
        new SCoroutine(TimeCounterController(scoroutine));
    }

    IEnumerator TimeCounter()
    {
        while(true)
        {
            print(Time.time);
            yield return null;
        }
    }

    IEnumerator TimeCounterController(SCoroutine scoroutine)
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            scoroutine.Pause();
            yield return new WaitForSeconds(1f);
        }
    }
}
