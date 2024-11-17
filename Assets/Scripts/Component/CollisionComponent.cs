using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Manager;

namespace Component
{
    public class CollisionComponent : MonoBehaviour
    {
        [SerializeField] private float _detectDistance = 0.1f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Rect _collisionRect;
    
        public LayerMask LayerMask => _layerMask;
        public float DetectDistance => _detectDistance;
        public float Width => _collisionRect.width;
        public float Height => _collisionRect.height;
        public Rect CollisionRect => _collisionRect;
    
        public event Action<GameObject> OnCollisionEnter;
        public event Action<GameObject> OnCollisionExit;
        private HashSet<GameObject> _collidedObjects = new();
        private void Awake()
        {
            AutoSetRect();
        }
        public void OnEnable()
        {
            _collidedObjects.Clear();
            if(CollisionManager.Instance) CollisionManager.Instance.RegisterCollisionComponent(this);
        }
    
        public void OnDisable()
        {
            if(CollisionManager.Instance) CollisionManager.Instance.UnregisterCollisionComponent(this);
        }
        public virtual void OnEntityCollisionEnter(GameObject other)
        {
            if (_collidedObjects.Contains(other))
            {
                return;
            }
            _collidedObjects.Add(other);
            Debug.Log($"{gameObject.name} collided with {other.name}");
            OnCollisionEnter?.Invoke(other);
        }

        public virtual void OnEntityCollisionExit(GameObject other)
        {
            if (_collidedObjects.Contains(other))
            {
                _collidedObjects.Remove(other);
            }
            else return;
            Debug.Log($"{gameObject.name} stopped colliding with {other.name}");
            OnCollisionExit?.Invoke(other);
        }
    
        [ContextMenu("Auto Set Rect")]
        public void AutoSetRect()
        {
            var size = _spriteRenderer.size;
            var scale = transform.lossyScale;
            _collisionRect = new Rect(0, 0, size.x * scale.x, size.y * scale.y);
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var position = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(position, new Vector3(_collisionRect.width, _collisionRect.height, 0));
       
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, _detectDistance);
        }
        #endif
    }

}
