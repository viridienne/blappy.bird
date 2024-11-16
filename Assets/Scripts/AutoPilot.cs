using System;
using System.Collections;
using System.Collections.Generic;
using Component;
using Entity;
using Manager;
using UnityEngine;

public class AutoPilot : MonoBehaviour
{
    [SerializeField] private CollisionComponent _collisionComponent;
    [SerializeField] private PlayerEntity _playerEntity;
    [SerializeField] private float _dangerY;
    [SerializeField] private bool _isAutoPilot;
    [SerializeField] private float _heightGap;
    public bool IsAutoPilot
    {
        get => _isAutoPilot;
        set => _isAutoPilot = value;
    }
    
    private void Update()
    {
        if(!_isAutoPilot) return;
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

        var nearestCheckPoint = CheckPointsManager.Instance.GetClosestCheckpoint(playerPosition, bounds/2);
        if (nearestCheckPoint == null) return false;
        var (lower, upper) = nearestCheckPoint.GetGap();
        var position = nearestCheckPoint.transform.position;
        var horizontalGap = nearestCheckPoint.GetHorizontalGap();
        // bool isUnder = playerPosition.y + _heightGap < position.y + (lower.y + upper.y) / 2;
        bool isUnder = playerPosition.y + _heightGap < position.y;
        if(isUnder)
        {
            return true;
        }

        if (playerPosition.x - _collisionComponent.CollisionRect.width/2 >= (position.x + horizontalGap.left.x) && playerPosition.x + _collisionComponent.CollisionRect.width/2 <= (position.x + horizontalGap.right.x ))
        {
            if (playerPosition.y < position.y - (_heightGap/2)) return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawLine(position, position + Vector3.right * _heightGap);
    }
}
