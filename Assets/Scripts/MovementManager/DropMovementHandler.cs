using GamePlay.Board;
using GamePlay.Drops;
using GamePlay.StateMachine;
using System;
using UnityEngine;

public class DropMovementHandler
{
    public static Action<DropMovementHandler> OnSpawn;
    public static Action<DropMovementHandler> OnDespawn;

    private Drop _drop;
    private Tile _currentTile;
    private bool _hasDownNeighbour;
    private bool _shouldFall;
    private int _frameCount;
    private float _startSpeed = 2f;
    private float _acceleration = 5f;
    private float _currentSpeed;

    public DropMovementHandler(Drop drop)
    {
        _drop = drop;
        OnSpawn?.Invoke(this);
    }

    public void UpdateTile(Tile tile)
    {
        _currentTile = tile;
        _hasDownNeighbour = _currentTile.GetNeighbour(Neighbour.Down) != null;
    }

    public void FixedUpdate()
    {
        if (!_hasDownNeighbour) return;

        if (_shouldFall == false && _currentTile.GetNeighbour(Neighbour.Down).CanAcceptDrop()
            && _currentTile.CanGiveDrop())
        {
            _currentTile.GiveDrop();
            _currentTile.GetNeighbour(Neighbour.Down).RecieveDrop();
            _shouldFall = true;
        }
        else
        {
            _frameCount = 0;
            _currentSpeed = _startSpeed;
            _shouldFall = false;
        }

        if (_shouldFall)
        {
            Debug.Log("fixed updating drop movement");
            _frameCount++;
            _currentSpeed += _acceleration * Mathf.Sqrt(_frameCount);
            _drop.transform.Translate(Vector3.down * -_currentSpeed);

            if (_currentTile.Position.y - _drop.transform.position.y <= 0.5f)
            {
                _currentTile = _currentTile.GetNeighbour(Neighbour.Down);
                _currentTile.AcceptDropTemprorary(_drop);
            }
            else if (_drop.transform.position.y - _currentTile.Position.y <= 0.05f)
            {
                _currentTile.GetNeighbour(Neighbour.Up).ReleaseDrop();
                _currentTile.AcceptDropFromFall(_drop);
                _frameCount = 0;
                _currentSpeed = _startSpeed;
                _shouldFall = false;
            }
        }
    }

    public void Reset()
    {
        _currentTile = null;
        _currentSpeed = _startSpeed;
        _frameCount = 0;
        OnDespawn.Invoke(this);
    }
}
