using System;
using UnityEngine;

public class StartEndAnimations : MonoBehaviour
{
    public event Action StartAnim;
    public event Action EndAnim;

    public void StartAnimEvent()
    {
        StartAnim?.Invoke();
    }

    public void EndAnimEvent()
    {
        EndAnim?.Invoke();
    }
}
