using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    private int highScore = 0;
    private int currentScore = 0;
    private int currentBonus = 100;
    private float bonusTimer;
    private int currentLevel = 1;
    private int currentBallOnBar = 1;
    private int maxBallCount = 3;

    ObjectivesScript objectiveScript;
    BarScript barScript;
    SpawnScript spawnScript;
    CameraScript cameraScript;
    MapGeneratorScript mapGeneratorScript;
    PauseScript pauseScript;
    LeaderboardManagerScript leaderboardManagerScript;

    [SerializeField]
    private TextMeshProUGUI bonusText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI ballOnBarText;

    [SerializeField]
    private TextMeshProUGUI endGameText;

    [SerializeField]
    private TextMeshProUGUI highScoreText;

    [SerializeField]
    private List<Light2D> lights;

    [SerializeField]
    private Light2D globalLight;

    public void PlayerScored()
    {
        scoreText.text = (currentScore += currentBonus).ToString();

        if (currentLevel % 7 == 0)
        {
            maxBallCount++;
            ballOnBarText.text = $"{currentBallOnBar} / {maxBallCount}";
        }

        foreach (var light in lights)
        {
            light.color = new Color32(0, 255, 0, 255);
            light.intensity = 1;
            light.pointLightOuterRadius = 20;
            Invoke(nameof(SpotlightReset), 7);
        }

        if (currentLevel % 10 == 0)
        {
            currentLevel++;
            PlayerWon();
            return;
        }

        var ballScript = FindObjectOfType<BallScript>();
        currentLevel++;
        currentBonus = currentLevel * 100;
        bonusText.text = currentBonus.ToString();

        ballScript.Invoke(nameof(ballScript.DoneCountingScore), 3);
        objectiveScript.SetNextObjective();
    }

    public void SpotlightReset()
    {
        foreach (var light in lights)
        {
            light.color = new Color32(255, 255, 255, 255);
            light.intensity = 0.5f;
            light.pointLightOuterRadius = 10;
        }

        globalLight.intensity = Constants.Map.GlobalLightIntensity;
    }

    public void PlayerMissed()
    {
        if (currentBallOnBar == maxBallCount)
        {
            PlayerLost();
            return;
        }

        currentBallOnBar++;

        ballOnBarText.text = $"{currentBallOnBar} / {maxBallCount}";

        var ballScript = FindObjectOfType<BallScript>();
        ballScript.Invoke(nameof(ballScript.DoneCountingScore), 3);

        globalLight.intensity = 0.3f;

        foreach (var light in lights)
        {
            light.color = new Color32(255, 0, 0, 255);
            light.intensity = 1;
            light.pointLightOuterRadius = 20;
            Invoke(nameof(SpotlightReset), 1);
        }
    }

    public void PlayerWon()
    {
        spawnScript.MoveSpawn();
        var nextObjectives = mapGeneratorScript.SpawnMap();

        cameraScript.follow = true;
        barScript.GoToEnd();
        objectiveScript.AddNewObjectives(nextObjectives, currentLevel);

    }

    public void PlayerLost()
    {
        endGameText.text = "GAME OVER!";

        if (currentScore > highScore)
        {
            highScore = currentScore;
            highScoreText.text = highScore.ToString();
            endGameText.text += "\nHIGH SCORE!";

            leaderboardManagerScript.SetLeaderBoardEntry(highScore);
        }

        pauseScript.GameOver();
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Awake()
    {
        leaderboardManagerScript = FindObjectOfType<LeaderboardManagerScript>();
    }

    private void Start()
    {
        bonusTimer = Constants.Score.BonusTimerEasy;
        bonusText.text = currentBonus.ToString();
        ballOnBarText.text = $"{currentBallOnBar} / {maxBallCount}";

        objectiveScript = FindObjectOfType<ObjectivesScript>();
        barScript = FindObjectOfType<BarScript>();
        spawnScript = FindObjectOfType<SpawnScript>();
        cameraScript = FindObjectOfType<CameraScript>();
        mapGeneratorScript = FindObjectOfType<MapGeneratorScript>();
        pauseScript = FindObjectOfType<PauseScript>();

        highScore = leaderboardManagerScript.HighScore;
        highScoreText.text = highScore.ToString();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    currentLevel += 10;
        //    PlayerWon();
        //}

        if (currentBonus <= 0)
        {

            if (currentBallOnBar == maxBallCount)
            {
                PlayerLost();
                return;
            }

            currentBallOnBar++;
            ballOnBarText.text = $"{currentBallOnBar} / {maxBallCount}";

            currentBonus = currentLevel * 100;
            bonusText.text = currentBonus.ToString();
            bonusTimer = Constants.Score.BonusTimerEasy;

            return;
        }

        if (bonusTimer > 0)
        {
            bonusTimer -= Time.deltaTime;
        }
        else
        {
            currentBonus -= 10;

            if (currentBonus > 200)
            {
                bonusTimer = Constants.Score.BonusTimerHard;
            }
            else
            {
                bonusTimer = Constants.Score.BonusTimerEasy;
            }

            bonusText.text = currentBonus.ToString();
        }
    }
}
