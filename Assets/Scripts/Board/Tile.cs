using GamePlay.Drops;
using GamePlay.SpawnSystem;
using GamePlay.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Board
{
    public class Tile : MonoBehaviour
    {
        public Drop CurrentDrop { get; private set; }
        public Vector3 Position => transform.position;
        public int X { get; private set; }
        public int Y { get; private set; }

        private BoardManager _board;
        private DropSpawner _spawner;

        private Dictionary<Neighbour, Tile> _neighbours = new();

        public StateManager StateManager { get; private set; }

        public void Initialize(BoardManager boardManager, Vector2Int indecies)
        {
            _board = boardManager;
            X = indecies.x;
            Y = indecies.y;

            transform.SetParent(boardManager.transform);
            transform.localPosition = new Vector3(indecies.x, indecies.y, 0f);
            gameObject.name = $"Tile[{Position.x}][{Position.y}]";

            StateManager = new StateManager();
            StateManager.AddState(new TileIsEmptyState());
            StateManager.AddState(new TileHasDropState());
            StateManager.AddState(new TileIsGivingDropState());
            StateManager.AddState(new TileIsRecievingDropState());
            StateManager.AddState(new TileDropIsPoppingState());
        }

        public void AddSpawner()
        {
            _spawner = gameObject.AddComponent<DropSpawner>();
        }

        public void AcceptDrop(Drop drop)
        {
            CurrentDrop = drop;
            CurrentDrop.GetInTile(this);
            StateManager.ChangeState(typeof(TileHasDropState));
        }

        public void GiveDrop()
        {
            StateManager.ChangeState(typeof(TileIsGivingDropState));
        }

        public void RecieveDrop()
        {
            StateManager.ChangeState(typeof(TileIsRecievingDropState));
        }

        public void PopDrop()
        {
            StateManager.ChangeState(typeof(TileDropIsPoppingState));
        }

        public void ReleaseDrop()
        {
            StateManager.ChangeState(typeof(TileIsEmptyState));
        }

        public bool CanAcceptDrop()
        {
            //Change to state
            return CurrentDrop == null;
        }

        public void SetNeighbour(Neighbour side, Tile tile)
        {
            if (_neighbours.ContainsKey(side))
            {
                _neighbours[side] = tile;
            }
            else
            {
                _neighbours.Add(side, tile);
            }
        }

        public Tile GetNeighbour(Neighbour side)
        {
            return _neighbours.TryGetValue(side, out var tile) ? tile : null;
        }
    }
}