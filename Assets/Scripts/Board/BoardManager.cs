using GamePlay.Colors;
using GamePlay.Factory;
using System.Collections;
using UnityEngine;

namespace GamePlay.Board
{
    public class BoardManager : MonoBehaviour
    {
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }

        private Tile[,] _tiles;

        private DropFactory _dropFactory;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);
            _dropFactory = DropFactory.Instance;

            InitializeBoard();
            GenerateBoard();
            FillBoard();
        }

        private void InitializeBoard()
        {
            _tiles = new Tile[Width, Height];
        }

        private void GenerateBoard()
        {
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    var tile = _dropFactory.GetTile();
                    tile.Initialize(this, new Vector2(i, j));
                    _tiles[i, j] = tile;
                }
            }
        }

        private void FillBoard()
        {
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    var randomColorId = Random.Range(0, 4);
                    IColor color = randomColorId switch
                    {
                        0 => new Red(),
                        1 => new Green(),
                        2 => new Blue(),
                        3 => new Yellow(),
                    };

                    var drop = _dropFactory.GetColoredDrop(color);
                    _tiles[i, j].AcceptDrop(drop);
                }
            }
        }
    }
}