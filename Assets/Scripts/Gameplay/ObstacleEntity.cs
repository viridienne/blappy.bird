using Component;
using Manager;
using UnityEngine;

namespace Entity
{
    public class ObstacleEntity : Entity
    {
        private Camera _mainCamera;
        [SerializeField] private CheckpointComponent _checkpointComponent;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private Vector2 _direction = Vector2.left;
        public override void OnEnable()
        {
            base.OnEnable();
            _mainCamera = Camera.main;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (GameManager.Instance.CurrentGameState != GameState.Playing) return;
            
            EntityTransform.Translate(_direction * (_speed * deltaTime));

            var horGap = _checkpointComponent.GetHorizontalGap();
            if (_mainCamera.WorldToViewportPoint(horGap.right).x < 0)
            {
                Release();
            }
        }
    }
}

