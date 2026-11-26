using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameStartingModule : MonoBehaviour
{
    // все модули игры для обновления
    List<IUpdaterModule> ModuleUpdate;

    [SerializeField] PlayerController playerController;

    InputMudule inputMudule;
    void Start()
    {
        inputMudule = new();
        inputMudule.Initialization();

        ModuleUpdate = new List<IUpdaterModule>();

        playerController.Initialization();

        ModuleUpdate.Add(inputMudule);
        ModuleUpdate.Add(playerController);

        AddGameCycle();
    }

    void AddGameCycle()
    {
        GetComponent<MainGameCycle>().StartUpdateGame(ModuleUpdate.ToArray());
    }
}
