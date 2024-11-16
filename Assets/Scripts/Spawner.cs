using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate = 1f;
    [SerializeField] private Vector2 _spawnRange = new Vector2(-1, 1);
    
    private EntityPool _entityPool;
    
    private void Awake()
    {
        _entityPool = GetComponent<EntityPool>();
        
    }

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0, _spawnRate);
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(Spawn));
    }

    public void Spawn()
    {
        var entity = _entityPool.Get();
        entity.EntityTransform.position = new Vector2(transform.position.x, Random.Range(_spawnRange.x, _spawnRange.y));
    }
}
