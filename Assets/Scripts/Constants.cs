namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Constants
    {
        public const float AudioFadeTime = 0.1f;

        public static class VolumeLevels
        {
            public const int Music = 1;
            public const float BallInHole = 0.2f;
            public const float MaxBallRolling = 1f;
            public const float MaxBallHittingBarClick = 0.6f;
            public const float Bar = 0.3f;
            public const float WrongBuzzer = 0.2f;
            public const float MenuMove = 1f;
            public const float MenuSelect = 1f;
        }

        public static class Sounds
        {
            public const string BallRolling = "ballRolling";
            public const string BallHittingBarClick = "click";
            public const string BallInHole = "ballInHole";
            public const string Music = "musicShort";
            public const string WrongBuzzer = "wrongBuzzer";
            public const string MenuMove = "menuMove";
            public const string MenuSelect = "menuSelect";
            public const string BarMovingDown = "down";
            public const string BarMovingUp = "up";
        }

        public static class Layers
        {
            public const int Objectives = 6;
            public const int Obstacles = 7;
        }

        public static class BallSettings
        {
            public const float MaxPitch = 1.3f;
            public const float MinPitch = 1.1f;
            public const float RollingThreshold = 0.5f;
            public const float MaxSpeed = 5;
            public const float ObstacleAttraction = 0.01f;
        }

        public static class BarSettings
        {
            public const float ControlSpeed = 2.2f;      
            public const float RotationSpeed = 11f;
            public const float MaxAngle = 12f;
            public const float MinAngle = -12f;
            public const float DeadZone = 0.9f;
            public const float IdleTimer = 5f;
        }

        public static class Map
        {
            public const int ObjectiveSpawnCount = 11;
            public const int MaxTry = 2000;

            public const float Right = 8.2f;
            public const float Left = -8.2f;
            public const float GlobalLightIntensity = 0.77f;
        }

        public static class Score
        {
            public const int BonusTimerEasy = 4;
            public const int BonusTimerHard = 2;
        }

        public static class VolumeTypes
        {
            public const string Master = "master";
            public const string Music = "music";
            public const string Sfx = "sfx";
        }
        
    }
}
