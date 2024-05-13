using Assets.Scripts;
using Assets.Scripts.Extensions;
using System.Collections;
using UnityEngine;

public class BarScript : MonoBehaviour
{
    public BoxCollider2D barCollider;
    public Rigidbody2D rb;

    private bool leftKeyWasPressed;
    private bool rightKeyWasPressed;
    private bool keysReleased;
    private bool goToStart = false;
    private bool stopBar = false;
    private bool touchingWall;

    private Vector2 leftInputDirection;
    private Vector2 rightInputDirection;
    private float inputDirection = 0;
    private float barRotation = 0;
    private float currentRotation = 0f;
    private float idleTimer = Constants.BarSettings.IdleTimer;
    private bool barIdle = false;

    private Vector2 startPosition;
    private Quaternion startRotaion;

    private SpawnScript spawnScript;

    private AudioManager audioManager;
    private MapGeneratorScript mapGeneratorScript;

    private string sound = string.Empty;
    private PlayerControls controls;

    public void GoToStart()
    {
        goToStart = true;
        currentRotation = startRotaion.eulerAngles.z;
        leftInputDirection = new();
        rightInputDirection = new();
        inputDirection = 0;
        barRotation = 0;
    }

    public void GoToEnd()
    {
        startPosition.y += MapGeneratorScript.MAP_SIZE;
        GoToStart();
    }

    public void StopBar()
    {
        stopBar = true;
    }

    public void UnlockBar()
    {
        stopBar = false;
    }

    public bool BarStopped()
    {
        return stopBar;
    }

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Game.Enable();
    }

    private void OnDisable()
    {
        controls.Game.Disable();
    }

    private void Start()
    {
        startPosition = transform.position;
        startRotaion = transform.rotation;
        currentRotation = startRotaion.eulerAngles.z;

        spawnScript = FindObjectOfType<SpawnScript>();
        audioManager = FindObjectOfType<AudioManager>();
        mapGeneratorScript = FindObjectOfType<MapGeneratorScript>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!goToStart || !stopBar)
        {
            if (controls.Game.LeftUp.IsPressed())
            {
                LeftUp();
            }
            else if (controls.Game.LeftDown.IsPressed())
            {
                LeftDown();
            }
            else
            {
                leftKeyWasPressed = false;
            }

            if (controls.Game.RightUp.IsPressed())
            {
                RightUp();
            }
            else if (controls.Game.RightDown.IsPressed())
            {
                RightDown();
            }
            else
            {
                rightKeyWasPressed = false;
            }

            if (controls.Game.LeftUp.WasReleasedThisFrame()
                && controls.Game.LeftDown.WasReleasedThisFrame()
                && controls.Game.RightUp.WasReleasedThisFrame()
                && controls.Game.RightDown.WasReleasedThisFrame())
            {
                keysReleased = true;
            }

            if (leftKeyWasPressed && rightKeyWasPressed)
            {
                if(leftInputDirection != rightInputDirection)
                {
                    barRotation = (Constants.BarSettings.RotationSpeed + 8) * rightInputDirection.y;
                }
                else
                {
                    barRotation = 0;
                }
            }

            if (idleTimer > 0)
            {
                idleTimer -= Time.deltaTime;
            }
            else if (idleTimer > -0.1f)
            {
                idleTimer -= Time.deltaTime;
                barIdle = true;
                
            }
            else
            {
                idleTimer = Constants.BarSettings.IdleTimer;
            }
        }
    }

    private void FixedUpdate()
    {

        if (goToStart)
        {
            audioManager.Play(Constants.Sounds.BarMovingDown, Constants.VolumeLevels.Bar);
            var step = (Constants.BarSettings.ControlSpeed + 3) * Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotaion, step*3);
            transform.position = Vector2.MoveTowards(transform.position, startPosition, step);

            if (Vector2.Distance(transform.position, startPosition) < 0.001f && Quaternion.Angle(transform.rotation, startRotaion) <= 0.001f)
            {
                goToStart = false;
                spawnScript.Spawn();
                audioManager.FadeAudioOut(Constants.Sounds.BarMovingDown, Constants.AudioFadeTime);
                Invoke(nameof(UnlockBar), 1f);
            }

            return;
        }

        if ((leftKeyWasPressed || rightKeyWasPressed) && !stopBar)
        {
            idleTimer = Constants.BarSettings.IdleTimer;
            if (!touchingWall)
            {
                var rotation = Mathf.Clamp((barRotation * Time.deltaTime) + currentRotation, Constants.BarSettings.MinAngle, Constants.BarSettings.MaxAngle);
                currentRotation = rotation;
                transform.rotation = Quaternion.Euler(0, 0, rotation);
            }

            if (sound != Constants.Sounds.BarMovingUp && inputDirection > 0)
            {
                sound = Constants.Sounds.BarMovingUp;
            }
            else if (sound != Constants.Sounds.BarMovingDown && inputDirection < 0)
            {
                sound = Constants.Sounds.BarMovingDown;
            }

            if ((currentRotation.Between(Constants.BarSettings.MinAngle, Constants.BarSettings.MaxAngle) && leftKeyWasPressed != rightKeyWasPressed) || (leftKeyWasPressed && rightKeyWasPressed && leftInputDirection == rightInputDirection))
            {
                var speed = Constants.BarSettings.ControlSpeed;
                if (leftKeyWasPressed && rightKeyWasPressed)
                {
                    speed += 2;
                }

                var newPos = gameObject.transform.position + speed * Time.deltaTime * new Vector3(0, inputDirection, 0);

                if (newPos.y >= startPosition.y - 1 && newPos.y <= mapGeneratorScript.GetTopOfMap() + 1)
                {
                    touchingWall = false;
                    audioManager.Play(sound, Constants.VolumeLevels.Bar);
                    gameObject.transform.position = newPos;
                }
                else
                {
                    touchingWall = true;
                }
            }
            else
            {
                audioManager.FadeAudioOut(sound, Constants.AudioFadeTime);
            }
        }
        else if (keysReleased)
        {
            keysReleased = false;
            audioManager.FadeAudioOut(sound, Constants.AudioFadeTime);

        }

        if (barIdle && !stopBar)
        {
            var newPos = gameObject.transform.position + 4.2f * Time.deltaTime * new Vector3(0, 1, 0);

            if (newPos.y >= startPosition.y - 1 && newPos.y <= mapGeneratorScript.GetTopOfMap() + 1)
            {
                touchingWall = false;
                audioManager.Play(Constants.Sounds.BarMovingUp, Constants.VolumeLevels.Bar);
                gameObject.transform.position = newPos;
            }
            else
            {
                touchingWall = true;
            }

            barIdle = false;
        }
    }

    private void LeftUp()
    {
        leftKeyWasPressed = true;
        leftInputDirection = Vector2.up;
        inputDirection = 1f;
        barRotation = -Constants.BarSettings.RotationSpeed;
    }

    private void LeftDown()
    {
        leftKeyWasPressed = true;
        leftInputDirection = Vector2.down;
        inputDirection = -1f;
        barRotation = Constants.BarSettings.RotationSpeed;
    }

    private void RightUp()
    {
        rightInputDirection = Vector2.up;
        inputDirection = 1f;
        rightKeyWasPressed = true;
        barRotation = Constants.BarSettings.RotationSpeed;
    }

    private void RightDown()
    {
        rightInputDirection = Vector2.down;
        inputDirection = -1f;
        rightKeyWasPressed = true;
        barRotation = -Constants.BarSettings.RotationSpeed;
    }

}
