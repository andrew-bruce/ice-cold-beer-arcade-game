using Assets.Scripts;
using Assets.Scripts.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BallScript : MonoBehaviour
{
    private CircleCollider2D ball;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool droppedToPipe = false;
    private BarScript barScript;
    private AudioManager audioManager;
    private ScoreScript scoreScript;
    public GameObject shinePrefab;
    private GameObject shine;

    private bool rolling;
    private float speed;
    private bool countingScore = false;
    private Vector3 positionOnBall = new(0f, 0.2f);

    public void DoneCountingScore()
    {
        countingScore = false;
    }

    private void Start()
    {
        ball = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); 
        barScript = FindObjectOfType<BarScript>();
        audioManager = FindObjectOfType<AudioManager>();
        scoreScript = FindObjectOfType<ScoreScript>();

        shine = Instantiate(shinePrefab, transform.position, transform.rotation);
    }

    private void Update()
    {
        shine.transform.position = transform.position + positionOnBall;

        if (rb.velocity.x >= Constants.BallSettings.RollingThreshold || rb.velocity.x <= -Constants.BallSettings.RollingThreshold)
        {
            rolling = true;
        }
        else
        {
            rolling = false;
        }

        speed = Math.Abs(rb.velocity.x);
    }

    private void FixedUpdate()
    {
        if (rolling)
        {
            audioManager.Play(Constants.Sounds.BallRolling, 0);

            var scaledVelocityVolume = Mathf.Clamp(speed, 0, Constants.BallSettings.MaxSpeed)
                .Remap(0, Constants.BallSettings.MaxSpeed, 0, Constants.VolumeLevels.MaxBallRolling);

            var scaledVelocityPitch = Mathf.Clamp(speed, 0, Constants.BallSettings.MaxSpeed)
                .Remap(0, Constants.BallSettings.MaxSpeed, Constants.BallSettings.MinPitch, Constants.BallSettings.MaxPitch);

            audioManager.SetVolume(Constants.Sounds.BallRolling, scaledVelocityVolume);
            audioManager.SetPitch(Constants.Sounds.BallRolling, scaledVelocityPitch);
        }
        else if (audioManager.IsPlaying(Constants.Sounds.BallRolling))
        {
            audioManager.FadeAudioOut(Constants.Sounds.BallRolling, Constants.AudioFadeTime);
        }
    }

    // When ball enters hole trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        // checks if the ball is fully contained by the trigger
        bool ballContained = collision.bounds.Contains(ball.bounds.min) && collision.bounds.Contains(ball.bounds.max);

        if (collision.gameObject.layer == Constants.Layers.Obstacles)
        {
            if (!countingScore && !ballContained && ball.bounds.min.y > collision.bounds.min.y && collision.bounds.Contains(ball.bounds.center) && ball.bounds.max.y < collision.bounds.max.y)
            {
                var newPos = new Vector2(collision.transform.position.x, gameObject.transform.position.y);
                gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.localPosition, newPos, Constants.BallSettings.ObstacleAttraction);
            }


            // player has missed
            if (ballContained && !countingScore)
            {
                barScript.StopBar();
                audioManager.Play(Constants.Sounds.WrongBuzzer, Constants.VolumeLevels.WrongBuzzer);
                countingScore = true;
                scoreScript.PlayerMissed();
                audioManager.Play(Constants.Sounds.BallInHole, Constants.VolumeLevels.BallInHole);
                barScript.GoToStart();
                StartCoroutine(FadeOut());
            }
        }
        else  if (collision.gameObject.layer == Constants.Layers.Objectives)
        {
            // player has scored
            if (ballContained && !countingScore)
            {
                barScript.StopBar();
                audioManager.Play(Constants.Sounds.Music, Constants.VolumeLevels.Music);
                countingScore = true;
                scoreScript.PlayerScored();
                audioManager.Play(Constants.Sounds.BallInHole, Constants.VolumeLevels.BallInHole);
                barScript.GoToStart();
                StartCoroutine(FadeOut());
            }
        }
    }

    private IEnumerator FadeOut()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        float counter = 0;
        float duration = 0.2f;

        //Get current color
        Color spriteColor = spriteRenderer.color;
        Light2D shineLight = shine.GetComponentInChildren<Light2D>();

        while (counter < duration)
        {
            counter += Time.deltaTime;
            //Fade from 1 to 0
            float alpha = Mathf.Lerp(1, 0, counter / duration);

            //Change alpha only
            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);

            if (shineLight.intensity > 0)
            {
                shineLight.intensity -= 0.2f;
            }

            //Wait for a frame
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 11 && speed > 1)
        {
            var scaledVelocityVolume = Mathf.Clamp(speed, 0, Constants.BallSettings.MaxSpeed).Remap(0, Constants.BallSettings.MaxSpeed, 0, Constants.VolumeLevels.MaxBallHittingBarClick);
            audioManager.Play(Constants.Sounds.BallHittingBarClick, scaledVelocityVolume);
            audioManager.Stop(Constants.Sounds.BallRolling);
        }

        if (!droppedToPipe && collision.gameObject.layer == 10)
        {
            audioManager.Play(Constants.Sounds.BallHittingBarClick, Constants.VolumeLevels.MaxBallHittingBarClick);
            rb.gravityScale = 15;
            droppedToPipe = true;
        }
    }
}
