using Unity.Cinemachine;
using UnityEngine;

public class CameraEffctController : IUpdaterModule
{
    readonly float cameraRollRight;
    readonly float cameraRollLeft;
    readonly float cameraFovRunning;
    readonly float cameraRollDef;
    readonly float cameraFovDef;
    readonly CinemachineCamera camera;
    private float _currentCameraRoll;
    private float _currentCameraFov;
    private float _inputMoveData;
    private MoveStatus _inputRunningData;

    public CameraEffctController(float cameraRollRight, float cameraRollLeft, float cameraFovRunning, 
        float cameraRollDef, float cameraFovDef, float currentCameraRoll, float currentCameraFov, 
        CinemachineCamera camera)
    {
        this.cameraRollRight = cameraRollRight;
        this.cameraRollLeft = cameraRollLeft;
        this.cameraFovRunning = cameraFovRunning;
        this.cameraRollDef = cameraRollDef;
        this.cameraFovDef = cameraFovDef;
        _currentCameraRoll = currentCameraRoll;
        _currentCameraFov = currentCameraFov;
        this.camera = camera;
    }

    public void UpdateMod(float delta)
    {
        // получение данных 
        _inputMoveData = InputMudule.instance.movementData.x;
        _inputRunningData = MovePlayerModule.MoveStatusPlayer;

        // проверка перемещения игрока в стороны - перенести все настройки в конфиг обджекс
        if(_inputMoveData > 0f)
        {
            _currentCameraRoll = Mathf.Lerp(_currentCameraRoll, 
                cameraRollRight, 1 - Mathf.Exp(-10f * delta));
        }
        else if(_inputMoveData < 0f)
        {
            _currentCameraRoll = Mathf.Lerp(_currentCameraRoll, 
                cameraRollLeft, 1 - Mathf.Exp(-10f * delta));
        }
        else
        {
            _currentCameraRoll = Mathf.Lerp(_currentCameraRoll, 
                cameraRollDef, 1 - Mathf.Exp(-10f * delta));
        }

        // проверка на бег игрока
        if (_inputRunningData == MoveStatus.run)
        {
            _currentCameraFov = Mathf.Lerp(_currentCameraFov, 
                cameraFovRunning, 1 - Mathf.Exp(-10f * delta));
        }
        else
        {
            _currentCameraFov = Mathf.Lerp(_currentCameraFov, 
                cameraFovDef, 1 - Mathf.Exp(-10f * delta));
        }

        // передача информации о изменение состояния камеры
        camera.Lens.FieldOfView = _currentCameraFov;
        camera.Lens.Dutch = _currentCameraRoll;
    }
}
