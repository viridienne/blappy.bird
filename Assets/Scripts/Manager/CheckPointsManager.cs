using System.Collections.Generic;
using Component;
using UnityEngine;

namespace Manager
{
    public class CheckPointsManager : BaseSingletonMono<CheckPointsManager>
    {

        private HashSet<CheckpointComponent> _checkpoints = new();

        public void RegisterCheckpoint(CheckpointComponent checkpoint)
        {
            if (!_checkpoints.Contains(checkpoint))
            {
                _checkpoints.Add(checkpoint);
            }
        }

        public void UnregisterCheckpoint(CheckpointComponent checkpoint)
        {
            if (_checkpoints.Contains(checkpoint))
            {
                _checkpoints.Remove(checkpoint);
            }
        }

        public CheckpointComponent GetClosestCheckpoint(Vector3 position, float horGap)
        {
            CheckpointComponent closestCheckpoint = null;
            float closestDistance = float.MaxValue;
            foreach (var checkpoint in _checkpoints)
            {
                var distance = Vector3.Distance(position, checkpoint.transform.position);
                var horizontalGap = checkpoint.GetHorizontalGap();
                if (distance < closestDistance)
                {
                    if (position.x + horGap > horizontalGap.right.x)
                    {
                        continue;
                    }

                    closestDistance = distance;
                    closestCheckpoint = checkpoint;
                }
            }

            return closestCheckpoint;
        }
    }
}
