using UnityEngine;

namespace Entity
{
    public class PlayerEntity : Entity
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _gravity = 9.81f;

        private Vector2 _direction;
        
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            
            Move();
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);
            _direction.y -= _gravity * fixedDeltaTime;
        }

        void Jump()
        {
            _direction = Vector2.up * _jumpForce;
        }
        
        void Move()
        {
            EntityTransform.Translate(_direction * (_speed * Time.deltaTime));
        }
    }
}

