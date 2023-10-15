using GamePlay.Board;
using GamePlay.Colors;
using GamePlay.Drops;
using GamePlay.Factory;
using System;
using UnityEngine;

namespace GamePlay.SpawnSystem
{
    public class DropSpawner : MonoBehaviour
    {
        private DropFactory _dropFactory;
        private Tile _tile;

        private void Awake()
        {
            _dropFactory = DropFactory.Instance;
        }

        public void Init(Tile tile)
        {
            _tile = tile;
        }

        public void SpawnDrop()
        {
            var drop = GetRandomColoredDrop();
            drop.transform.position = transform.position + Vector3.up;
            _tile.RecieveDrop();
            _tile.AcceptDropTemprorary(drop);
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