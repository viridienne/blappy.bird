using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
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
        GameManager.Instance.OnAfterGameStateChanged += OnAfterGameStateChanged;
    }

    private void OnAfterGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                StartSpawn();
                break;
            default:
                StopSpawn();
                break;
        }
    }

    private void StartSpawn()
    {
        InvokeRepeating(nameof(Spawn), 0, _spawnRate);
    }
    private void StopSpawn()
    {
        _entityPool.ReleaseAll();
        CancelInvoke(nameof(Spawn));
    }
    private void OnDestroy()
    {
        StopSpawn();
        if(GameManager.Instance) GameManager.Instance.OnAfterGameStateChanged -= OnAfterGameStateChanged;
    }

    public void Spawn()
    {
        var entity = _entityPool.Get();
        entity.EntityTransform.position = new Vector2(transform.position.x, Random.Range(_spawnRange.x, _spawnRange.y));
    }
}
