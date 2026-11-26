using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class MovePlayerModule : IUpdaterModule, IInitializationModule
{
    readonly CinemachineCamera cameraPlayer;
    readonly float walkSpeed;
    readonly float runSpeed;
    readonly Animator animCam;
    readonly CharacterController characterControllerPlayer;
    readonly GunsController gunsController;
    static public MoveStatus MoveStatusPlayer { get; private set; }

    private Vector2 _movePlayerData;
    private Vector3 _movement = Vector3.zero;

    private float _currentSpeed;
    private float _verticalSpeed;
    private bool _playerFly;
    private Vector3 _power;

    private float _slidePower = 0.5f;


    public MovePlayerModule(CharacterController characterControllerPlayer, float walkSpeed, 
        float runSpeed, CinemachineCamera cameraPlayer, Animator animCam,
        GunsController gunsController) {
        this.walkSpeed = walkSpeed;
        this.runSpeed = runSpeed;
        this.characterControllerPlayer = characterControllerPlayer;
        this.cameraPlayer = cameraPlayer;
        this.animCam = animCam;
        this.gunsController = gunsController;
    }
    public void Initialization()
    {
        //MoveAction = InputSystem.actions.FindAction("Move");
        gunsController.ShotGunPlayerEvent += RecoilPlayer;
    }
  

    public void UpdateMod(float delta)
    {

        _movePlayerData = InputMudule.instance.movementData.normalized;
        InitializationStatus();
        CheckStatus();

        _movement = ((cameraPlayer.transform.forward * _currentSpeed * _movePlayerData.y 
            + cameraPlayer.transform.right * _currentSpeed * _movePlayerData.x));

        if (!_playerFly)
        {
            CheckJump();
        }
        else
            CheckLanding();

        CheckSlide();

        if (_power != Vector3.zero)
        {
            _power = Vector3.Lerp(_power, Vector3.zero, 1f - Mathf.Exp(-5f * Time.deltaTime));
            _movement += new Vector3(_power.x, 0f, _power.z);
        }

        _verticalSpeed = Mathf.Clamp(_verticalSpeed, -20, 20);
        characterControllerPlayer.Move(new Vector3(_movement.x, _verticalSpeed, _movement.z) * Time.deltaTime);
    }

    private void CheckJump()
    {
        if (characterControllerPlayer.isGrounded && 
            InputMudule.instance.jumpngData && !_playerFly)
        {
            _verticalSpeed = 0f;
            _verticalSpeed += 8f;
            _playerFly = true;
        }
    }

    private void RecoilPlayer(GunRef gunsArray)
    {
        _power = -cameraPlayer.transform.forward * gunsArray.recoilMove;
        _power.y = 0f;
    }

    private void CheckLanding()
    {
        if(characterControllerPlayer.isGrounded)
            _playerFly = false;
    }

    private void CheckStatus()
    {
        if (MoveStatusPlayer == MoveStatus.run)
        {
            animCam.SetBool("Run", true);
            if (_currentSpeed < runSpeed)
                _currentSpeed += 9f * Time.deltaTime;
            else
                _currentSpeed = runSpeed;
        }
        else if (MoveStatusPlayer == MoveStatus.walk)
        {
            animCam.SetBool("Run", false);
            animCam.SetBool("Walk", true);
            if (_currentSpeed > walkSpeed)
                _currentSpeed -= 15f * Time.deltaTime;
            else if (_currentSpeed < walkSpeed)
                _currentSpeed += 15f * Time.deltaTime;
            else
                _currentSpeed = walkSpeed;
        }
        else if (MoveStatusPlayer == MoveStatus.idle)
        {
            animCam.SetBool("Walk", false);
            animCam.SetBool("Run", false);
            _currentSpeed = 0f;
        }
    }

    private void CheckSlide()
    {
        RaycastHit hit;

        if (!characterControllerPlayer.isGrounded)
        {
            _verticalSpeed -= 14f * Time.deltaTime;
        }
        else if (Physics.SphereCast(characterControllerPlayer.transform.position,
            characterControllerPlayer.radius,
            -characterControllerPlayer.transform.up, 
            out hit, characterControllerPlayer.height / 2f) && !_playerFly)
        {
            float angle = Vector3.Angle(hit.normal, 
                characterControllerPlayer.transform.up);
            if (angle > 45f || !CheckIsGrounded())
            {
                if(_slidePower < 5f)
                {
                    _slidePower += 0.2f * Time.deltaTime;
                }
                _movement += hit.normal * _slidePower;
            }
            else
            {
                _verticalSpeed = -2f;
                _slidePower = 0.5f;
            }
        }
    }
    
    private bool CheckIsGrounded()
    {
        if(Physics.Raycast(characterControllerPlayer.transform.position,
            -characterControllerPlayer.transform.up, characterControllerPlayer.height / 2f))
            return true;
        else
            return false;
    }

    private void InitializationStatus()
    {
        if(_movePlayerData != Vector2.zero && InputMudule.instance.runningData && 
            InputMudule.instance.movementData.y > 0)
        {
            MoveStatusPlayer = MoveStatus.run;
        }
        else if(_movePlayerData != Vector2.zero)
        {
            MoveStatusPlayer = MoveStatus.walk;
        }
        else
        {
            MoveStatusPlayer = MoveStatus.idle;
        }
    }
}


public enum MoveStatus
{
    idle = 0,
    walk = 1,
    run = 2
}

