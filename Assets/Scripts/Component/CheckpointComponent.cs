using Manager;
using UnityEngine;

namespace Component
{
    public class CheckpointComponent : MonoBehaviour
    {
        [SerializeField] private float _horizontalGap;
        [SerializeField] private float _gap;

        private void OnEnable()
        {
            if (CheckPointsManager.Instance) CheckPointsManager.Instance.RegisterCheckpoint(this);
        }

        private void OnDisable()
        {
            if (CheckPointsManager.Instance)
                CheckPointsManager.Instance.UnregisterCheckpoint(this);
        }

        public (Vector2 lower, Vector2 upper) GetGap()
        {
            var pos = transform.position;
            var lower = pos + Vector3.down * _gap;
            var upper = pos + Vector3.up * _gap;
            return (lower, upper);
        }

        public (Vector2 left, Vector2 right) GetHorizontalGap()
        {
            var pos = transform.position;
            var left = pos + Vector3.left * _horizontalGap;
            var right = pos + Vector3.right * _horizontalGap;
            return (left, right);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var (lower, upper) = GetGap();
            Gizmos.DrawLine(lower, upper);
            
            Gizmos.color = Color.blue;
            var (left, right) = GetHorizontalGap();
            Gizmos.DrawLine(left, right);
        }
#endif
    }
}

