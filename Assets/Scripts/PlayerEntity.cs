using System;
using Component;
using UnityEngine;

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
            if (other.CompareTag("Obstacle"))
            {
                Debug.LogError("Game Over");
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (Input.GetKeyDown(KeyCode.Space) && !_autoPilot.IsAutoPilot)
            {
                Jump();
            }

            _direction.y -= _gravity * _mass * deltaTime;

            Move(deltaTime);
        }

        // public override void OnFixedUpdate(float fixedDeltaTime)
        // {
        //     base.OnFixedUpdate(fixedDeltaTime);
        // }

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

