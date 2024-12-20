using Entity;
using Manager;
using UnityEngine;

namespace Component
{
    public class AutoPilot : MonoBehaviour
    {
        [SerializeField] private CollisionComponent _collisionComponent;
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private float _dangerY;
        [SerializeField] private float _heightGap;

        public bool IsAutoPilot => GameManager.Instance != null && GameManager.Instance.IsAutoPilot;

        private void Update()
        {
            if (!IsAutoPilot) return;
            if (_playerEntity.EntityTransform.position.y < _dangerY)
            {
                _playerEntity.Jump();
                return;
            }

            if (IsObstacleAhead())
            {
                _playerEntity.Jump();
            }
        }

        public bool IsObstacleAhead()
        {
            if (CheckPointsManager.Instance == null) return false;
            var bounds = _collisionComponent.CollisionRect.width;
            var playerPosition = _playerEntity.EntityTransform.position;

            var nearestCheckPoint = CheckPointsManager.Instance.GetClosestCheckpoint(playerPosition, bounds / 2);
            if (nearestCheckPoint == null) return false;
            var obstaclePos = nearestCheckPoint.transform.position;
            var horizontalGap = nearestCheckPoint.GetHorizontalGap();
            bool isUnder = playerPosition.y + _heightGap < obstaclePos.y;
            if (isUnder)
            {
                return true;
            }

            if (playerPosition.x - _collisionComponent.CollisionRect.width / 2 >= (obstaclePos.x + horizontalGap.left.x) &&
                playerPosition.x + _collisionComponent.CollisionRect.width / 2 <= (obstaclePos.x + horizontalGap.right.x))
            {
                if (playerPosition.y < obstaclePos.y - (_heightGap / 2))
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var position = transform.position;
            Gizmos.DrawLine(position, position + Vector3.right * _heightGap);
        }
#endif
    }
}
