using System;
using Component;
using JSAM;
using Manager;
using UnityEngine;
using Utility;

namespace Entity
{
    public class PlayerEntity : Entity
    {
        [SerializeField] private Transform _model;
        [Header("Settings")]
        [SerializeField] private float _upAngle = 30;
        [SerializeField] private float _downAngle = -30;
        [SerializeField] private float _mass = 5f; //think of it like mass
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _gravity = 9.81f;
        
        [Header("Components")]
        [SerializeField] private Animator _animator;
        [SerializeField] private AutoPilot _autoPilot;
        [SerializeField] private CollisionComponent _collisionComponent;
        
        [Header("Sound")]
        [SerializeField] private SoundFileObject _jumpSound;
        [SerializeField] private SoundFileObject _collisionSound;
        
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
                    _playedCollisionSound = false;
                    _animator.speed = 1;
                    _currentMass = _mass;
                    _currentGravity = _gravity;
                    break;
                case GameState.Lose:
                    _animator.speed = 0;
                    break;
                case GameState.Starting:
                    _model.rotation = Quaternion.Euler(0, 0, 0);
                    _animator.speed = 1;
                    EntityTransform.position = Vector2.zero;
                    _currentGravity = 0;
                    _direction = Vector2.zero;
                    _currentMass = 0;
                    
                    gameObject.SetActive(false);
                    gameObject.SetActive(true);
                    break;
            }
        }

        private bool _playedCollisionSound;

        private void OnCollision(GameObject other)
        {
            switch (other.tag)
            {
                case TagConstant.Obstacle:
                    PlayCollisionSound();
                    GameManager.Instance.GameOver();
                    break;
                case TagConstant.Checkpoint:
                    CheckPointsManager.Instance.Checkpoint();
                    break;
                case TagConstant.Ground:
                    PlayCollisionSound();
                    GameManager.Instance.GameOver();
                    _currentGravity = 0;
                    _direction = Vector2.zero;
                    _currentMass = 0;
                    _model.rotation = Quaternion.Euler(0, 0, _downAngle);
                    break;
            }
        }

        private void PlayCollisionSound()
        {
            if (_playedCollisionSound) return;
            AudioManager.PlaySound(_collisionSound);
            _playedCollisionSound = true;
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
            SetRotation(_direction.y);
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
            AudioManager.PlaySound(_jumpSound);
        }
        
        private void Move(float deltaTime)
        {
            EntityTransform.Translate(_direction * deltaTime);
        }
        
        private void SetRotation(float y)
        {
            if(GameManager.Instance.CurrentGameState != GameState.Playing) return;
            _model.rotation = y switch
            {
                0 => Quaternion.Euler(0, 0, 0),
                > 0 => Quaternion.Euler(0, 0, _upAngle),
                < 0 => Quaternion.Euler(0, 0, _downAngle),
                _ => _model.rotation
            };
        }
    }
}

