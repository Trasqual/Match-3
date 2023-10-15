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
        private bool _shouldFall;
        private int _frameCount;
        private float _startSpeed = 0.05f;
        private float _acceleration = 0.01f;
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
        }

        public void FixedUpdate()
        {
            if (_currentTile == null) return;

            if (_currentTile.CanGiveDrop()
                && _currentTile.GetNeighbour(Neighbour.Down) != null && _currentTile.GetNeighbour(Neighbour.Down).CanAcceptDrop())
            {
                _shouldFall = true;

                _currentTile.GiveDrop();
                _currentTile.GetNeighbour(Neighbour.Down).RecieveDrop();
                _target = _currentTile.GetNeighbour(Neighbour.Down).transform;
            }

            if (_shouldFall)
            {
                _currentSpeed += _acceleration * Mathf.Sqrt(_frameCount);
                _drop.transform.Translate(Vector3.down * _currentSpeed);

                if ((_currentTile.Position.y - _drop.transform.position.y) >= 0.4f)
                {
                    _currentTile.ReleaseDrop();
                    _currentTile.GetNeighbour(Neighbour.Down).AcceptDropTemprorary(_drop);
                }

                if ((_drop.transform.position.y - _target.position.y) <= 0.01f)
                {
                    _currentTile.AcceptDropFromFall(_drop);
                    _currentSpeed = _startSpeed;
                    _shouldFall = false;
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