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

        public override void Awake()
        {
            base.Awake();
            _collisionComponent.OnCollisionEnter += OnCollision;
        }

        private void OnDestroy()
        {
            _collisionComponent.OnCollisionEnter -= OnCollision;
        }
        
        private void OnCollision(GameObject other)
        {
            if (other.CompareTag(TagConstant.Obstacle))
            {
                GameManager.Instance.GameOver();
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (!_autoPilot.IsAutoPilot)
            {
                DetectInput();
            }

            _direction.y -= _gravity * _mass * deltaTime;

            Move(deltaTime);
        }

        private void DetectInput()
        {
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

