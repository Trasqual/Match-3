using GamePlay.Drops;
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

        private Dictionary<Neighbour, Tile> _neighbours = new();

        public void Initialize(BoardManager boardManager, Vector2Int indecies)
        {
            _board = boardManager;
            X = indecies.x;
            Y = indecies.y;
            transform.SetParent(boardManager.transform);
            transform.localPosition = new Vector3(indecies.x, indecies.y, 0f);
            gameObject.name = $"Tile[{Position.x}][{Position.y}]";
        }

        public void AcceptDrop(Drop drop)
        {
            CurrentDrop = drop;

            CurrentDrop.GetInTile(this);
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