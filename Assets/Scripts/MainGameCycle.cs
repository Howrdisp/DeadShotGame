using UnityEngine;

public class MainGameCycle : MonoBehaviour
{
    static public bool StartGameCycle;
    private IUpdaterModule[] updateModuls;

    public void StartUpdateGame(IUpdaterModule[] updateModuls)
    {
        this.updateModuls = updateModuls;
        StartGameCycle = true;
    }

    void Update()
    {
        if (StartGameCycle)
        {
            for (int i = 0; i < updateModuls.Length; i++)
            {
                updateModuls[i].UpdateMod(Time.deltaTime);
            }
        }
    }
}
