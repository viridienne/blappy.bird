using System.Collections.Generic;
using Component;
using UnityEngine;
using Utility;

namespace Manager
{
    public class CollisionManager : BaseSingletonMono<CollisionManager>
    {
        private readonly List<CollisionComponent> _collisionComponents = new();

        public void RegisterCollisionComponent(CollisionComponent collisionComponent)
        {
            if (!_collisionComponents.Contains(collisionComponent))
            {
                _collisionComponents.Add(collisionComponent);
            }
        }

        public void UnregisterCollisionComponent(CollisionComponent collisionComponent)
        {
            if (_collisionComponents.Contains(collisionComponent))
            {
                _collisionComponents.Remove(collisionComponent);
            }
        }

        private void Update()
        {
            for (int i = _collisionComponents.Count - 1; i >= 0; i--)
            {
                for (int j = _collisionComponents.Count - 1; j >= 0; j--)
                {
                    var collisionComponent = _collisionComponents[i];
                    var otherCollisionComponent = _collisionComponents[j];
                    if (collisionComponent.gameObject.GetInstanceID() == otherCollisionComponent.gameObject.GetInstanceID())
                    {
                        continue;
                    }
                    if (Vector3.Distance(collisionComponent.transform.position,
                            otherCollisionComponent.transform.position) >
                        collisionComponent.DetectDistance)
                    {
                        continue;
                    }

                    if (collisionComponent.LayerMask !=
                        (collisionComponent.LayerMask | (1 << otherCollisionComponent.gameObject.layer)))
                    {
                        continue;
                    }

                    var context = new CollisionUtil.CollisionContext
                    {
                        PositionA = collisionComponent.transform.position,
                        SizeA = new Vector2(collisionComponent.CollisionRect.width,
                            collisionComponent.CollisionRect.height),
                        PositionB = otherCollisionComponent.transform.position,
                        SizeB = new Vector2(otherCollisionComponent.CollisionRect.width,
                            otherCollisionComponent.CollisionRect.height)
                    };
                    if (CollisionUtil.CheckAABB(context))
                    {
                        collisionComponent.OnEntityCollisionEnter(otherCollisionComponent.gameObject);
                    }
                    else
                    {
                        collisionComponent.OnEntityCollisionExit(otherCollisionComponent.gameObject);
                    }
                }
            }
        }

        public CollisionComponent GetNearestCollisionComponent(CollisionComponent collisionComponent)
        {
            var nearestCollisionComponent = collisionComponent;
            var nearestDistance = float.MaxValue;
            foreach (var otherCollisionComponent in _collisionComponents)
            {
                if (collisionComponent.gameObject.GetInstanceID() == otherCollisionComponent.gameObject.GetInstanceID())
                {
                    continue;
                }

                var distance = Vector3.Distance(collisionComponent.transform.position,
                    otherCollisionComponent.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestCollisionComponent = otherCollisionComponent;
                }
            }

            return nearestCollisionComponent;
        }
    }
}
