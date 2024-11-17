using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Pool;

namespace Entity
{
    public interface IEntity
    {
        public Transform EntityTransform { get; }
        public GameObject EntityGameObject { get; }
    
        void RegisterEntity();
        void UnregisterEntity();
    
        void OnUpdate(float deltaTime);
        void OnFixedUpdate(float fixedDeltaTime);
        void OnLateUpdate();
        
        void SetPool(IObjectPool<Entity> pool);
        void Release();
    }
    
    public class Entity : MonoBehaviour, IEntity
    {
        public IObjectPool<Entity> Pool
        {
            get; 
            private set;
        }
        public GameObject EntityGameObject => _entityGameObject;
        public Transform EntityTransform => _entityTransform;
        
        private Transform _entityTransform;
        private GameObject _entityGameObject;
    
        public void SetPool(IObjectPool<Entity> pool)
        {
            Pool = pool;
        }
        public virtual void RegisterEntity()
        {
            if(EntitiesManager.Instance) EntitiesManager.Instance.RegisterEntity(this);
        }

        public virtual void UnregisterEntity()
        {
            if(EntitiesManager.Instance) EntitiesManager.Instance.UnregisterEntity(this);
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }

        public virtual void OnFixedUpdate(float fixedDeltaTime)
        {
        }

        public virtual void OnLateUpdate()
        {
        }

        public virtual void Awake()
        {
            _entityGameObject = gameObject;
            _entityTransform = transform;
        }

        public virtual void OnEnable()
        {
            RegisterEntity();
        }

        public virtual void OnDisable()
        {
            UnregisterEntity();
        }
        
        public void Release()
        {
            Pool.Release(this);
        }
    }

}
