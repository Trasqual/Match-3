using GamePlay.Board;
using GamePlay.Colors;
using GamePlay.Drops;
using GamePlay.Visuals;
using UnityEngine;
using Utilities;

namespace GamePlay.Factory
{
    [DefaultExecutionOrder(-40)]
    public class DropFactory : Singleton<DropFactory>
    {
        private PrefabProvider _prefabProvider;
        private SpriteManager _spriteManager;

        protected override void Awake()
        {
            base.Awake();
            _prefabProvider = PrefabProvider.Instance;
            _spriteManager = SpriteManager.Instance;
        }

        public Drop GetColoredDrop(IColor color)
        {
            var drop = _prefabProvider.GetColoredDrop();
            drop.Initialize(color, _spriteManager.GetDropSprite(color));

            return drop;
        }

        public Tile GetTile()
        {
            var tile = _prefabProvider.GetTile();

            return tile;
        }
    }
}