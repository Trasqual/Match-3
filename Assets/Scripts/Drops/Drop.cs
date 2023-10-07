using GamePlay.Board;
using GamePlay.Colors;
using UnityEngine;

namespace GamePlay.Drops
{
    public class Drop : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _rend;

        public IColor Color { get; private set; }

        private Tile _tile;

        public void Initialize(IColor color, Sprite sprite)
        {
            Color = color;
            _rend.sprite = sprite;
        }

        public void GetInTile(Tile tile)
        {
            _tile = tile;
            transform.SetParent(_tile.transform, false);
            transform.localPosition = Vector3.zero;
        }
    }
}