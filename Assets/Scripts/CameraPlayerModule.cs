using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPlayerModule: IUpdaterModule, IInitializationModule
{
    public CinemachineCamera cameraPlayer;
    private CameraEffctController cameraEffctController;
    private GunsController gunsController;
    readonly float camSpeed;
    private Vector2 LookPlayerData;
    private float rotateCamX;
    private float rotateCamY;
    private bool blockRotateY;
    private float targetRotateY;

    public CameraPlayerModule( CinemachineCamera cameraPlayer, float camSpeed, GunsController gunsController)
    {
        this.cameraPlayer = cameraPlayer;
        this.camSpeed = camSpeed;
        this.gunsController = gunsController;
    }

    // получение отдачи от выстрела 
    private void RecoilPlayer(GunRef gunRef)
    {
        blockRotateY = true;
        targetRotateY = rotateCamY - gunRef.recoilCam;
    }

    public void Initialization()
    {
        // эффекты камеры -- перенести в гейм обджекс
        cameraEffctController = new(-5f, 5f, 75f, 0f, 60f, 0f, 60f, cameraPlayer);
        gunsController.ShotGunPlayerEvent += RecoilPlayer;
    }

    public void UpdateMod(float delta)
    {
        LookPlayerData = InputMudule.instance.lookData * camSpeed; 

        rotateCamX += LookPlayerData.x;

        // проверка на отдачу и её отсутствие

        if(!blockRotateY)
            rotateCamY -= LookPlayerData.y;
        else
        {
            rotateCamY = Mathf.Lerp(rotateCamY, targetRotateY, 1f - Mathf.Exp(-20f * Time.deltaTime));

            if (Mathf.Abs(rotateCamY - targetRotateY) < 0.01f || rotateCamY <= -45f)
                blockRotateY = false;
        }

        rotateCamY = Mathf.Clamp(rotateCamY, -45f, 45f); // ограничитель поворота -- вывести в конфиг
        cameraPlayer.transform.rotation = Quaternion.Euler(rotateCamY, rotateCamX, 0f); 

        cameraEffctController.UpdateMod(delta); // обвноления эффнетов камеры
    }
}
