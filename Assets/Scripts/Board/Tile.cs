using GamePlay.Drops;
using GamePlay.SpawnSystem;
using GamePlay.StateMachine;
using Main.Gameplay.Core;
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
        private TileTouchHandler _touchHandler;

        private Dictionary<Neighbour, Tile> _neighbours = new();

        public StateManager StateManager { get; private set; }
        public bool HasSpawner { get; private set; }

        public string CurrentState;

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

            _touchHandler = new TileTouchHandler(this);
        }

        public void AddSpawner()
        {
            _spawner = gameObject.AddComponent<DropSpawner>();
            _spawner.Init(this);
            HasSpawner = true;
        }

        public void AcceptDrop(Drop drop)
        {
            CurrentDrop = drop;
            if (CurrentDrop != null)
            {
                CurrentDrop.GetInTile(this);
                StateManager.ChangeState(typeof(TileHasDropState));
            }
        }

        public void AcceptDropTemprorary(Drop drop)
        {
            CurrentDrop = drop;
            if (CurrentDrop != null)
                CurrentDrop.GetInTileTemprory(this);
        }

        public void AcceptDropFromFall(Drop drop)
        {
            AcceptDrop(drop);

            if (MatchFinder.FindMatches(this, out var tiles))
            {
                tiles.Add(this);
                foreach (var tile in tiles)
                {
                    tile.PopDrop();
                }
            }
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
            if (CurrentDrop != null)
                CurrentDrop.Pop(() => ReleaseDrop());
        }

        public void ReleaseDrop()
        {
            CurrentDrop = null;
            StateManager.ChangeState(typeof(TileIsEmptyState));
            if (_spawner != null)
                _spawner.SpawnDrop();
        }

        public bool CanGiveDrop()
        {
            return StateManager.CurrentState is TileHasDropState or TileIsRecievingDropState;
        }

        public bool CanAcceptDrop()
        {
            return StateManager.CurrentState is TileIsEmptyState or TileIsGivingDropState;
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

        public void TrySwap(Neighbour neighbour)
        {
            if (!CanSwap()) return;

            if (_neighbours.TryGetValue(neighbour, out var neighbourTile))
            {
                if (!neighbourTile.CanSwap()) return;

                StateManager.ChangeState(typeof(TileIsSwappingState));
                SwapHandler.TrySwap(this, neighbourTile);
            }
        }

        public bool CanSwap()
        {
            return StateManager.CurrentState is TileHasDropState or TileIsEmptyState;
        }

        private void FixedUpdate()
        {
            CurrentState = StateManager.CurrentState.ToString();
        }
    }
}