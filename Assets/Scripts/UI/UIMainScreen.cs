using System;
using Manager;
using TMPro;
using UnityEngine;
using UniRx;

namespace UI
{
    public class UIMainScreen : BaseUI
    {
        [SerializeField] private GameObject _startPhase;
        [SerializeField] private GameObject _playingPhase;
        [SerializeField] private TMP_Text _textScore;
        private IDisposable _disposable;
        public override void Show()
        {
            base.Show();
            GameManager.Instance.OnAfterGameStateChanged += OnAfterGameStateChanged;
            if (GameManager.Instance.CurrentGameState == GameState.Starting)
            {
                _startPhase.SetActive(true);
            }
            
            _disposable = CheckPointsManager.Instance.Point.Subscribe(SetScore);
        }

        private void SetScore(int value)
        {
            _textScore.SetText(value.ToString());
        }
        public override void Hide()
        {
            base.Hide();
            _disposable?.Dispose();
            GameManager.Instance.OnAfterGameStateChanged -= OnAfterGameStateChanged;
        }

        private void OnAfterGameStateChanged(GameState obj)
        {
            switch (obj)
            {
                case GameState.Playing:
                    _textScore.gameObject.SetActive(true);
                    _startPhase.SetActive(false);
                    _playingPhase.SetActive(true);
                    break;
                case GameState.Starting:
                    _textScore.gameObject.SetActive(false);
                    _startPhase.SetActive(true);
                    _playingPhase.SetActive(false);
                    break;
                default:
                    _startPhase.SetActive(false);
                    _playingPhase.SetActive(false);
                    break;
            }
        }

        public void OnButtonStart()
        {
            GameManager.Instance.OnButtonStart(false);
        }
        
        public void OnButtonAutoPilot()
        {
            GameManager.Instance.OnButtonStart(true);
        }
        
        public void OnButtonQuit()
        {
            GameManager.Instance.OnButtonQuit();
        }
    }
}

