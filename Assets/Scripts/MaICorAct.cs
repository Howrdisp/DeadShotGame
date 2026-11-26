using UnityEngine;

public class MainCorAct : MonoBehaviour
{
    public static MainCorAct main;

    private void Awake()
    {
        main = this;
    }
}
