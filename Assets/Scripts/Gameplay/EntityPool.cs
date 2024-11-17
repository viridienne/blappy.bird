using UnityEngine;
using UnityEngine.Pool;

public class EntityPool : MonoBehaviour
{
    [SerializeField] private Entity.Entity _entityPrefab;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private int _maxPoolSize = 100;
    private ObjectPool<Entity.Entity> _pool;

    public void Awake()
    {
        _pool = new ObjectPool<Entity.Entity>(Spawn, Get, Release, Destroy, _collectionCheck, _poolSize, _maxPoolSize);
    }

    public Entity.Entity Spawn()
    {
        var entity = Instantiate(_entityPrefab, transform);
        return entity;
    }

    public void Get(Entity.Entity entity)
    {
        entity.SetPool(_pool);
        entity.gameObject.SetActive(true);
    }
    
    public Entity.Entity Get()
    {
        return _pool.Get();
    }

    public void Release(Entity.Entity entity)
    {
        entity.gameObject.SetActive(false);
    }

    public void Destroy(Entity.Entity entity)
    {
        Destroy(entity.gameObject);
    }
    
    public void ReleaseAll()
    {
        _pool.Clear();
    }
}
