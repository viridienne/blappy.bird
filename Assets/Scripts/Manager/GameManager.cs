using System;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Manager
{
    public enum GameState
    {
        Init,
        Starting,
        Playing,
        Paused,
        Lose
    }

    public class GameManager : BaseSingletonMono<GameManager>
    {
        [ShowInInspector] public GameState CurrentGameState { get; private set; }

        public bool IsAutoPilot { get; private set; }
        public event Action<GameState> OnBeforeGameStateChanged;
        public event Action<GameState> OnAfterGameStateChanged;

        protected override void Awake()
        {
            base.Awake();
            
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            Debug.Log("Start Game");
            SetState(GameState.Init);
            SetState(GameState.Starting);
        }
        
        public void SetState(GameState state)
        {
            Debug.Log($"Before Game State: {state}");
            OnBeforeGameStateChanged?.Invoke(state);
            switch (state)
            {
                case GameState.Init:
                    HandleInit();
                    break;
                case GameState.Starting:
                    HandleStarting();
                    break;
                case GameState.Playing:
                    HandlePlaying();
                    break;
                case GameState.Paused:
                    HandlePaused();
                    break;
                case GameState.Lose:
                    HandleLose();
                    break;
            }

            CurrentGameState = state;
            Debug.Log($"After Game State: {state}");
            OnAfterGameStateChanged?.Invoke(state);
        }
        
        void HandleInit()
        {
            UIManager.Instance.ShowUI(UIKey.Full_MainScreen);
        }
        void HandleStarting()
        {
            
        }


        void HandlePlaying()
        {
        
        }
        void HandlePaused()
        {
        
        }
        void HandleLose()
        {
            Debug.LogError($"Game Over -> high score: {PlayerPrefs.GetInt("HighScore")}");
            UIManager.Instance.ShowUI(UIKey.Popup_GameOver, ui =>
            {
                if (ui is PopupGameOver popupGameOver)
                {
                    popupGameOver.OnRetry = ()=>
                    {
                        SetState(GameState.Starting);
                    };
                }
            });
        }
        
        public void GameOver()
        {
            if (CurrentGameState == GameState.Playing)
            {
                SetState(GameState.Lose);
            }
        }
        
        public void OnPause()
        {
            SetState(GameState.Paused);
        }
        
        public void OnButtonStart(bool isAutoPilot)
        {
            IsAutoPilot = isAutoPilot;
            SetState(GameState.Playing);
        }
        
        public void OnButtonQuit()
        {
            if (CurrentGameState == GameState.Playing)
            {
                GameOver();
            }
        }
    }
}

