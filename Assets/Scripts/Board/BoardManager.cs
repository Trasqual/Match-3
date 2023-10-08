using GamePlay.Colors;
using GamePlay.Drops;
using GamePlay.Events;
using GamePlay.Factory;
using GamePlay.SpawnSystem;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GamePlay.Board
{
    [DefaultExecutionOrder(0)]
    public class BoardManager : MonoBehaviour
    {

        [SerializeField] private BoardData Data;

        public int Width => Data.Width;
        public int Height => Data.Height;

        public Tile[,] Tiles { get; private set; }

        private DropFactory _dropFactory;

        private void Awake()
        {
            _dropFactory = DropFactory.Instance;

            InitializeBoard();
            GenerateBoard();
            FillBoard();
        }

        private void InitializeBoard()
        {
            Tiles = new Tile[Width, Height];
        }

        private void GenerateBoard()
        {
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    var tile = _dropFactory.GetTile();
                    tile.Initialize(this, new Vector2Int(i, j));
                    if (i > 0)
                    {
                        tile.SetNeighbour(Neighbour.Left, Tiles[i - 1, j]);
                        Tiles[i - 1, j].SetNeighbour(Neighbour.Right, tile);
                    }
                    if (j > 0)
                    {
                        tile.SetNeighbour(Neighbour.Down, Tiles[i, j - 1]);
                        Tiles[i, j - 1].SetNeighbour(Neighbour.Up, tile);
                    }
                    if (j == Height - 1)
                    {
                        tile.AddSpawner();
                    }
                    Tiles[i, j] = tile;
                }
            }
        }

        private void FillBoard()
        {
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    FillTile(Tiles[i, j]);
                }
            }

            EventManager.Instance.TriggerEvent<BoardFilledInitallyEvent>(new BoardFilledInitallyEvent { Data = Data });
        }

        private void FillTile(Tile tile)
        {
            do
            {
                if (tile.CurrentDrop != null)
                {
                    tile.CurrentDrop.RemoveSelf();
                }

                tile.AcceptDrop(GetRandomColoredDrop());

            } while (HasInitialMatch(tile));
        }

        private bool HasInitialMatch(Tile tile)
        {
            if (tile.X > 1)
            {
                var firstLeftNeighbour = tile.GetNeighbour(Neighbour.Left);
                var secondLeftNeighbour = tile.GetNeighbour(Neighbour.Left).GetNeighbour(Neighbour.Left);

                if (firstLeftNeighbour != null && secondLeftNeighbour != null
                    && firstLeftNeighbour.CurrentDrop.Color.GetType() == tile.CurrentDrop.Color.GetType()
                    && secondLeftNeighbour.CurrentDrop.Color.GetType() == tile.CurrentDrop.Color.GetType())
                {
                    return true;
                }
            }
            if (tile.Y > 1)
            {
                var firstDownNeighbour = tile.GetNeighbour(Neighbour.Down);
                var secondDownNeighbour = tile.GetNeighbour(Neighbour.Down).GetNeighbour(Neighbour.Down);
                if (firstDownNeighbour != null && secondDownNeighbour != null
                    && firstDownNeighbour.CurrentDrop.Color.GetType() == tile.CurrentDrop.Color.GetType()
                    && secondDownNeighbour.CurrentDrop.Color.GetType() == tile.CurrentDrop.Color.GetType())
                {
                    return true;
                }
            }
            return false;
        }

        private Drop GetRandomColoredDrop()
        {
            var randomColorId = UnityEngine.Random.Range(0, 4);
            IColor color = randomColorId switch
            {
                0 => new Red(),
                1 => new Green(),
                2 => new Blue(),
                3 => new Yellow(),
                _ => throw new NullReferenceException($"Requested a color that was out of the scope of 4 possible drops: {randomColorId}")
            };

            return _dropFactory.GetColoredDrop(color);
        }
    }
}