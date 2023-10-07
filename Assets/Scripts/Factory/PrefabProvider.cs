using GamePlay.Board;
using GamePlay.Drops;
using UnityEngine;
using Utilities;

namespace GamePlay.Factory
{
    public class PrefabProvider : Singleton<PrefabProvider>
    {
        [field: SerializeField] public Drop DropPrefab { get; private set; }
        [field: SerializeField] public Tile TilePrefab { get; private set; }

        public Drop GetColoredDrop()
        {
            return Instantiate(DropPrefab);
        }

        public Tile GetTile()
        {
            return Instantiate(TilePrefab);
        }
    }
}