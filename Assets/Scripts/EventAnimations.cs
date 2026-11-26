using UnityEngine;
using System;

public class EventAnimations : MonoBehaviour
{
    public event Action Play;

    public void PlayEvent()
    {
        Play.Invoke();
    }
}
