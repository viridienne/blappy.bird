using System.Collections.Generic;
using System.Linq;
using Entity;
using UnityEngine;

namespace Manager
{
    public class EntitiesManager : BaseSingletonMono<EntitiesManager>
    {
        private HashSet<IEntity> _entities = new HashSet<IEntity>();
    
        public void RegisterEntity(IEntity entity)
        {
            if (!_entities.Contains(entity))
            {
                _entities.Add(entity);
            }
        }
    
        public void UnregisterEntity(IEntity entity)
        {
            if (_entities.Contains(entity))
            {
                _entities.Remove(entity);
            }
        }
    
        private void Update()
        {
            for( int i = _entities.Count - 1; i >= 0; i--)
            {
                _entities.ElementAt(i).OnUpdate(Time.deltaTime);
            }
        }
    
        private void FixedUpdate()
        {
           for (int i = _entities.Count - 1; i >= 0; i--)
           {
               _entities.ElementAt(i).OnFixedUpdate(Time.fixedDeltaTime);
           }
        }
    
        private void LateUpdate()
        {
            for (int i = _entities.Count - 1; i >= 0; i--)
            {
                _entities.ElementAt(i).OnLateUpdate();
            }
        }
    }
}
