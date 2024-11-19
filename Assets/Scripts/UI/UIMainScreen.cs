using System;
using Manager;
using PrimeTween;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UniRx;
using Unity.Mathematics;

namespace UI
{
    public class UIMainScreen : BaseUI
    {
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private Transform _buttonQuit;
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

        private void OnDisable()
        {
            _disposable?.Dispose();
            GameManager.Instance.OnAfterGameStateChanged -= OnAfterGameStateChanged;
        }

        private void OnAfterGameStateChanged(GameState obj)
        {
            if(gameObject == null) return;
            
            switch (obj)
            {
                case GameState.Playing:
                    _startPhase.SetActive(false);
                    _playingPhase.SetActive(true);
                    
                    if(GameManager.Instance.IsAutoPilot)
                    {
                        PlayButtonQuitAnimation();
                    }
                    break;
                case GameState.Starting:
                    _startPhase.SetActive(true);
                    _playingPhase.SetActive(false);
                    break;
                default:
                    _startPhase.SetActive(false);
                    _playingPhase.SetActive(false);
                    break;
            }
        }
        
        [Button]
        public void PlayButtonQuitAnimation()
        {
            _buttonQuit.localRotation = quaternion.Euler(0, 0, 90);
            Tween.Rotation(_buttonQuit, Quaternion.identity, _duration, _curve, startDelay: 2f);
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

