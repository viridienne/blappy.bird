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
    
        public UnityAction<GameObject> OnCollisionEnter;
        public UnityAction<GameObject> OnCollisionExit;

        private void Awake()
        {
            AutoSetRect();
        }
        public void OnEnable()
        {
            if(CollisionManager.Instance) CollisionManager.Instance.RegisterCollisionComponent(this);
        }
    
        public void OnDisable()
        {
            if(CollisionManager.Instance) CollisionManager.Instance.UnregisterCollisionComponent(this);
        }
        public virtual void OnEntityCollisionEnter(GameObject other)
        {
            Debug.Log($"{gameObject.name} collided with {other.name}");
        }

        public virtual void OnEntityCollisionExit(GameObject other)
        {
            Debug.Log($"{gameObject.name} stopped colliding with {other.name}");
        }
    
        [ContextMenu("Auto Set Rect")]
        public void AutoSetRect()
        {
            var size = _spriteRenderer.size;
            var scale = transform.lossyScale;
            _collisionRect = new Rect(0, 0, size.x * scale.x, size.y * scale.y);
        }
        private void OnDrawGizmos()
        {
            var position = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(position, new Vector3(_collisionRect.width, _collisionRect.height, 0));
       
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, _detectDistance);
        }
    
    }

}
