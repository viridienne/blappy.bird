using System;
using Component;
using Manager;
using UnityEngine;
using Utility;

namespace Entity
{
    public class PlayerEntity : Entity
    {
        [SerializeField] private AutoPilot _autoPilot;
        [SerializeField] private float _mass = 5f; //think of it like mass
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _gravity = 9.81f;
        [SerializeField] private CollisionComponent _collisionComponent;
        private Vector2 _direction;
        private float _currentGravity;
        private float _currentMass;
        public override void Awake()
        {
            base.Awake();
            _collisionComponent.OnCollisionEnter += OnCollision;
        }

        private void OnDestroy()
        {
            _collisionComponent.OnCollisionEnter -= OnCollision;
        }

        private void Start()
        {
            GameManager.Instance.OnAfterGameStateChanged += OnAfterGameStateChanged;
        }

        private void OnAfterGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    _currentMass = _mass;
                    _currentGravity = _gravity;
                    break;
                case GameState.Lose:
                    _currentGravity = 0;
                    _direction = Vector2.zero;
                    _currentMass = 0;
                    break;
                case GameState.Starting:
                    EntityTransform.position = Vector2.zero;
                    _currentGravity = 0;
                    _direction = Vector2.zero;
                    _currentMass = 0;
                    
                    gameObject.SetActive(false);
                    gameObject.SetActive(true);
                    break;
            }
        }

        private void OnCollision(GameObject other)
        {
            switch (other.tag)
            {
                case TagConstant.Obstacle:
                    GameManager.Instance.GameOver();
                    break;
                case TagConstant.Checkpoint:
                    CheckPointsManager.Instance.Checkpoint();
                    break;
                case TagConstant.Ground:
                    GameManager.Instance.GameOver();
                    break;
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (!_autoPilot.IsAutoPilot)
            {
                DetectInput();
            }

            _direction.y -= _currentGravity * _currentMass * deltaTime;

            Move(deltaTime);
        }

        private void DetectInput()
        {
            if (GameManager.Instance is not { CurrentGameState: GameState.Playing }) return;
            
            if (Input.GetKeyDown(KeyCode.Space)) Jump();

            if (Input.touchCount <= 0) return;
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Jump();
            }
        }

        public void Jump()
        {
            _direction = Vector2.up * _jumpForce;
        }
        
        void Move(float deltaTime)
        {
            EntityTransform.Translate(_direction * deltaTime);
        }
    }
}

