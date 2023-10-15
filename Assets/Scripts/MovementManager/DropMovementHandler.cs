using GamePlay.Board;
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
        private float _startSpeed = 0.1f;
        private float _acceleration = 0.1f;
        private float _currentSpeed;

        private Tile _target;

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

            if (!_shouldFall && _currentTile.CanAcceptDrop() && _currentTile.HasSpawner)
            {
                _shouldFall = true;
                _target = _currentTile;
            }

            if (!_shouldFall && _currentTile.CanGiveDrop() && _currentTile.GetNeighbour(Neighbour.Down) != null && _currentTile.GetNeighbour(Neighbour.Down).CanAcceptDrop())
            {
                _shouldFall = true;
                _currentTile.GiveDrop();
                _currentTile.GetNeighbour(Neighbour.Down).RecieveDrop();
                _target = _currentTile.GetNeighbour(Neighbour.Down);
            }

            if (_shouldFall)
            {
                _currentSpeed += _acceleration * _frameCount;
                _drop.transform.Translate(Vector3.down * _currentSpeed);

                if ((_currentTile.Position.y - _drop.transform.position.y) > 0.4f)
                {
                    _currentTile.ReleaseDrop();
                    _currentTile.GetNeighbour(Neighbour.Down).AcceptDropTemprorary(_drop);
                }

                if ((_drop.transform.position.y - _target.Position.y) <= 0.01f)
                {
                    if (_currentTile.CanGiveDrop() && _currentTile.GetNeighbour(Neighbour.Down) != null && _currentTile.GetNeighbour(Neighbour.Down).CanAcceptDrop())
                    {
                        _target = _currentTile.GetNeighbour(Neighbour.Down);
                    }
                    else
                    {
                        _shouldFall = false;
                        _currentTile.AcceptDrop(_drop);
                        _currentSpeed = _startSpeed;
                    }
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