using GamePlay.Board;
using GamePlay.StateMachine;
using System;
using UnityEngine;

namespace GamePlay.Drops.Movement
{
    public class DropMovementHandler
    {
        public static Action<DropMovementHandler> OnSpawn;
        public static Action<DropMovementHandler> OnDespawn;

        private Drop _drop;
        private Tile _currentTile;
        private bool _hasDownNeighbour;
        private bool _shouldFall;
        private int _frameCount;
        private int _startSpeed = 1000;
        private int _acceleration = 120;
        private float _currentSpeed;

        private Transform _target;

        public DropMovementHandler(Drop drop)
        {
            _drop = drop;
            OnSpawn?.Invoke(this);
            _currentSpeed = _startSpeed;
        }

        public void UpdateTile(Tile tile)
        {
            _currentTile = tile;
            _hasDownNeighbour = _currentTile.GetNeighbour(Neighbour.Down) != null;
        }

        public void FixedUpdate()
        {
            if (_currentTile == null) return;

            if (!_shouldFall && _hasDownNeighbour && _currentTile.CanGiveDrop()
                && _currentTile.GetNeighbour(Neighbour.Down).CanAcceptDrop())
            {
                _target = _currentTile.GetNeighbour(Neighbour.Down).transform;
                Debug.Log(_target);
                _currentTile.GiveDrop();
                _currentTile.GetNeighbour(Neighbour.Down).RecieveDrop();
                _shouldFall = true;
            }

            if (_shouldFall)
            {
                if ((_currentTile.Position.y - _drop.transform.position.y) * 10000 >= 2250)
                {
                    _currentTile.ReleaseDrop();
                    _currentTile.GetNeighbour(Neighbour.Down).AcceptDropTemprorary(_drop);
                }

                _frameCount++;
                _currentSpeed += _acceleration * Mathf.Sqrt(_frameCount);
                _drop.transform.Translate(0, -_currentSpeed / 10000, 0);
                Debug.Log(_target);
                if ((_drop.transform.position.y - _target.position.y)*10000 <= 128)
                {
                    _target = null;
                    _frameCount = 0;
                    _currentSpeed = _startSpeed;
                    _shouldFall = false;
                    _currentTile.AcceptDropFromFall(_drop);
                    //Debug.Log(_currentTile + " : " + _currentTile.StateManager.CurrentState);
                    //Debug.Log(_currentTile.GetNeighbour(Neighbour.Down) + " : " + _currentTile.GetNeighbour(Neighbour.Down).StateManager.CurrentState);
                    //Debug.Log(_currentTile.GetNeighbour(Neighbour.Up) + " : " + _currentTile.GetNeighbour(Neighbour.Up).StateManager.CurrentState);
                }
            }
        }

        public void Reset()
        {
            _currentTile = null;
            _currentSpeed = _startSpeed;
            _shouldFall = false;
            _frameCount = 0;
            OnDespawn?.Invoke(this);
        }
    }
}