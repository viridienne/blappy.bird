using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Entity
{
    public class EntityPool : MonoBehaviour
    {
        [SerializeField] private Entity _entityPrefab;
        [SerializeField] private bool _collectionCheck = true;
        [SerializeField] private int _poolSize = 10;
        [SerializeField] private int _maxPoolSize = 100;
        private ObjectPool<Entity> _pool;
        private readonly List<Entity> _activeEntities = new();
        public void Awake()
        {
            _pool = new ObjectPool<Entity>(Spawn, Get, Release, Destroy, _collectionCheck, _poolSize, _maxPoolSize);
        }

        public Entity Spawn()
        {
            var entity = Instantiate(_entityPrefab, transform);
            return entity;
        }

        public void Get(Entity entity)
        {
            entity.SetPool(_pool);
            entity.gameObject.SetActive(true);
            _activeEntities.Add(entity);
        }
    
        public Entity Get()
        {
            return _pool.Get();
        }

        public void Release(Entity entity)
        {
            entity.gameObject.SetActive(false);
            _activeEntities.Remove(entity);
        }

        public void Destroy(Entity entity)
        {
            Destroy(entity.gameObject);
            _activeEntities.Remove(entity);
        }
    
        public void ReleaseAll()
        {
            for(int i = _activeEntities.Count - 1; i >= 0; i--)
            {
                _pool.Release(_activeEntities[i]);
            }
        }
    }

}
