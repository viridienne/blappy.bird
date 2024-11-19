using System;
using JSAM;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AutoPlayButton : MonoBehaviour
    {
        [SerializeField] public SoundFileObject _tapSound;
        [SerializeField] public Transform _target;
        [SerializeField] public Vector3 _zoomScale = new(1.25f, 1.25f, 1.25f);
        [SerializeField] public float _duration = 0.1f;
        private Tween _scaleTween;

        private Vector3 _initialScale;

        protected void Awake()
        {
            _initialScale = _target.localScale;
        }

        protected void OnDisable()
        {
            if(_scaleTween.isAlive) _scaleTween.Stop();
        }

        public void OnClick(Action onComplete)
        {
            if (_scaleTween.isAlive) _scaleTween.Stop();
            _target.localScale = _initialScale;
            if(_tapSound) AudioManager.PlaySound(_tapSound);

            if (_zoomScale != _initialScale)
            {
                _scaleTween = Tween.Scale(_target, _zoomScale, _duration, cycles: 2, cycleMode: CycleMode.Yoyo);
                _scaleTween.OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
            }
            else
            {
                onComplete?.Invoke();
            }
        }
    }
}

 