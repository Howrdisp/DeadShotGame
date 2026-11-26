using UnityEngine;
using UnityEngine.InputSystem;

public class InputMudule : IUpdaterModule, IInitializationModule
{
    public static InputMudule instance;
    InputAction movementDataCheck;
    InputAction lookDataCheck;
    InputAction runningDataCheck;
    InputAction jumpDataCheck;
    InputAction attackDataCheck;
    InputAction ChangeGunCheck;

    public Vector2 movementData, lookData;
    public bool runningData, jumpngData, attackData, changeGunData;

    public void Initialization()
    {
        instance = this;
        movementDataCheck = InputSystem.actions.FindAction("Move");
        lookDataCheck = InputSystem.actions.FindAction("Look");
        runningDataCheck = InputSystem.actions.FindAction("Running");
        jumpDataCheck = InputSystem.actions.FindAction("Jumping");
        attackDataCheck = InputSystem.actions.FindAction("Attack");
        ChangeGunCheck = InputSystem.actions.FindAction("ChangeGun");
    }

    public void UpdateMod(float delta)
    {
        ResetData();
        movementData = movementDataCheck.ReadValue<Vector2>();
        lookData = lookDataCheck.ReadValue<Vector2>();
        runningData = runningDataCheck.IsInProgress();
        jumpngData = jumpDataCheck.IsInProgress();
        attackData = attackDataCheck.IsInProgress();
        changeGunData = ChangeGunCheck.WasPressedThisFrame();
    }

    void ResetData()
    {
        movementData = Vector2.zero;
        lookData = Vector2.zero;
        runningData = false;
        jumpngData = false;
        attackData = false;
        changeGunData = false;
    }
}

