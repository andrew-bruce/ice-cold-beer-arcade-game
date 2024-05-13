namespace Assets.Scripts
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using Dan.Main;
    using System.Linq;
    using System;

    public class LeaderboardManagerScript : MonoBehaviour
    {
        public int HighScore;
        public static LeaderboardManagerScript instance;

        private const string publicLeaderBoardKey = "d79896c772a9369597c888aad012e2e133465465c881159d22251282ca918fcf";

        public void GetHighScore()
        {
            LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, (msg) =>
            {
                var topEntry = msg.OrderByDescending(m => m.Score).FirstOrDefault();

                HighScore = topEntry.Score;
            });
        }

        public void SetLeaderBoardEntry(int score)
        {
            LeaderboardCreator.UploadNewEntry(publicLeaderBoardKey, Guid.NewGuid().ToString(), score);
        }

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            GetHighScore();
        }
    }
}
