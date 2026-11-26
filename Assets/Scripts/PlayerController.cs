using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInitializationModule, IUpdaterModule
{
    // конфиг упралвения игрока -- перенести в гейм обджекс
    [SerializeField] CharacterController characterController;
    [SerializeField] CinemachineCamera cameraPlayer;
    [SerializeField] float CameraSpeed;
    [SerializeField] float MoveSpeedWalk;
    [SerializeField] float MoveSpeedRun;
    [SerializeField] Animator animCam;
    [SerializeField] AllTypesGuns allTypesGuns;

    // данные оружия пока одно
    [SerializeField] Animator gunAnimator;
    [SerializeField] ParticleSystem fireParticle;
    [SerializeField] StartEndAnimations startEndAnimationsGun;

    // оружия модели
    [SerializeField] GunsData[] All_guns_data;

    private CameraPlayerModule cameraPlayerModule;
    private MovePlayerModule movePlayerModule;
    private GunsController gunsController;

    public void Initialization()
    {
        gunsController = new(All_guns_data);
        gunsController.Initialization();
        cameraPlayerModule = new(cameraPlayer, CameraSpeed, gunsController);
        cameraPlayerModule.Initialization();
        movePlayerModule = new(characterController, MoveSpeedWalk, 
            MoveSpeedRun, cameraPlayerModule.cameraPlayer, animCam, gunsController);
        movePlayerModule.Initialization();
    }

    public void UpdateMod(float delta)
    {
        cameraPlayerModule.UpdateMod(delta);
        movePlayerModule.UpdateMod(delta);
        gunsController.UpdateMod(delta);
    }
}
