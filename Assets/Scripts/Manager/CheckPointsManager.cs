using System;
using System.Collections.Generic;
using Component;
using JSAM;
using UniRx;
using UnityEngine;

namespace Manager
{
    public class CheckPointsManager : BaseSingletonMono<CheckPointsManager>
    {
        [SerializeField] private SoundFileObject _checkpointSound;
        public ReactiveProperty<int> Point { get; private set; }

        private GameState _currentGameState
        {
            get
            {
                if (GameManager.Instance == null)
                {
                    return GameState.Init;
                }

                return GameManager.Instance.CurrentGameState;
            }
        }
        private readonly HashSet<CheckpointComponent> _checkpoints = new();

        protected override void Awake()
        {
            base.Awake();
            Point = new ReactiveProperty<int>();
        }

        private void Start()
        {
            GameManager.Instance.OnAfterGameStateChanged += OnAfterGameStateChanged;
            GameManager.Instance.OnBeforeGameStateChanged += OnBeforeGameStateChanged;
        }

        private void OnBeforeGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Lose:
                    if (Point.Value > PlayerPrefs.GetInt("HighScore"))
                    {
                        PlayerPrefs.SetInt("HighScore", Point.Value);
                    }
                    break;
            }
        }

        private void OnAfterGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    Point.Value= 0;
                    break;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnAfterGameStateChanged -= OnAfterGameStateChanged;
            GameManager.Instance.OnBeforeGameStateChanged -= OnBeforeGameStateChanged;
        }

        public void RegisterCheckpoint(CheckpointComponent checkpoint)
        {
            if (!_checkpoints.Contains(checkpoint))
            {
                _checkpoints.Add(checkpoint);
            }
        }

        public void UnregisterCheckpoint(CheckpointComponent checkpoint)
        {
            if (_checkpoints.Contains(checkpoint))
            {
                _checkpoints.Remove(checkpoint);
            }
        }

        public CheckpointComponent GetClosestCheckpoint(Vector3 position, float horGap)
        {
            CheckpointComponent closestCheckpoint = null;
            float closestDistance = float.MaxValue;
            foreach (var checkpoint in _checkpoints)
            {
                var distance = Vector3.Distance(position, checkpoint.transform.position);
                var horizontalGap = checkpoint.GetHorizontalGap();
                if (distance < closestDistance)
                {
                    if (position.x + horGap > horizontalGap.right.x)
                    {
                        continue;
                    }

                    closestDistance = distance;
                    closestCheckpoint = checkpoint;
                }
            }

            return closestCheckpoint;
        }

        public void Checkpoint()
        {
            if(_currentGameState != GameState.Playing) return;
            Point.Value++;
            AudioManager.PlaySound(_checkpointSound);
        }
    }
}
