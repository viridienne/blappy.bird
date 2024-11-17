using System;
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
        public GameState CurrentGameState { get; private set; }

        public event Action<GameState> OnBeforeGameStateChanged;
        public event Action<GameState> OnAfterGameStateChanged;
        
        public void SetState(GameState state)
        {
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
            OnAfterGameStateChanged?.Invoke(state);
        }
        
        void HandleInit()
        {
        
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
            Debug.LogError("Game Over");
        }
        
        public void GameOver()
        {
            SetState(GameState.Lose);
        }
        
        public void OnPause()
        {
            SetState(GameState.Paused);
        }
    }
}

