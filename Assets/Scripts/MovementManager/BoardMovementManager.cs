using GamePlay.Drops.Movement;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Board
{
    [DefaultExecutionOrder(-10)]
    public class BoardMovementManager : MonoBehaviour
    {
        private List<DropMovementHandler> _movementHandlers = new();

        private void Awake()
        {
            DropMovementHandler.OnSpawn += AddDrop;
            DropMovementHandler.OnDespawn += RemoveDrop;
        }

        private void OnDestroy()
        {
            DropMovementHandler.OnSpawn -= AddDrop;
            DropMovementHandler.OnDespawn -= RemoveDrop;
        }

        private void AddDrop(DropMovementHandler dropMovementHandler)
        {
            if (!_movementHandlers.Contains(dropMovementHandler))
            {
                _movementHandlers.Add(dropMovementHandler);
            }
        }

        private void RemoveDrop(DropMovementHandler dropMovementHandler)
        {
            if (_movementHandlers.Contains(dropMovementHandler))
            {
                _movementHandlers.Remove(dropMovementHandler);
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _movementHandlers.Count; i++)
            {
                _movementHandlers[i].FixedUpdate();
            }
        }
    }
}