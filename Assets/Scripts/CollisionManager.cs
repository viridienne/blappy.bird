using System;
using System.Collections;
using System.Collections.Generic;
using Component;
using UnityEngine;

namespace Manager
{
    public class CollisionManager : BaseSingletonMono<CollisionManager>
{
    private HashSet<CollisionComponent> _collisionComponents = new HashSet<CollisionComponent>();
    
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

    private void FixedUpdate()
    {
        foreach (var collisionComponent in _collisionComponents)
        {
            foreach (var otherCollisionComponent in _collisionComponents)
            {
                if (collisionComponent.gameObject.GetInstanceID() == otherCollisionComponent.gameObject.GetInstanceID())
                {
                    continue;
                }
                
                //check detection distance
                if (Vector3.Distance(collisionComponent.transform.position, otherCollisionComponent.transform.position) >
                    collisionComponent.DetectDistance)
                {
                    continue;
                }
                
                //check if contain layer mask
                if(collisionComponent.LayerMask != (collisionComponent.LayerMask | (1 << otherCollisionComponent.gameObject.layer)))
                {
                    continue;
                }
                var context = new CollisionHelper.CollisionContext
                {
                    PositionA = collisionComponent.transform.position,
                    SizeA = new Vector2(collisionComponent.CollisionRect.width, collisionComponent.CollisionRect.height),
                    PositionB = otherCollisionComponent.transform.position,
                    SizeB = new Vector2(otherCollisionComponent.CollisionRect.width, otherCollisionComponent.CollisionRect.height)
                };
                if (CollisionHelper.CheckAABB(context))
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
}

}
