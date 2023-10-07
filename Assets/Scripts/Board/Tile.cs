using GamePlay.Drops;
using UnityEngine;

namespace GamePlay.Board
{
    public class Tile : MonoBehaviour
    {
        public Drop CurrentDrop { get; private set; }
        public Vector2 Position { get; private set; }

        private BoardManager _board;

        public void Initialize(BoardManager boardManager, Vector2 position)
        {
            Position = position;
            _board = boardManager;
            transform.position = Position;
        }

        public void AcceptDrop(Drop drop)
        {
            if (CanAcceptDrop())
                CurrentDrop = drop;

            CurrentDrop.GetInTile(this);
        }

        public bool CanAcceptDrop()
        {
            //Change to state
            return CurrentDrop == null;
        }
    }
}