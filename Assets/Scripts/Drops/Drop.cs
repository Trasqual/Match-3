using DG.Tweening;
using GamePlay.Board;
using GamePlay.Colors;
using System;
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

        public void Pop(Action onAnimationEnd)
        {
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOPunchScale(Vector3.one * 1.1f, 0.2f));
            s.Append(transform.DOScale(Vector3.zero, 0.1f));
            s.OnComplete(() => onAnimationEnd?.Invoke());
        }

        public void RemoveSelf()
        {
            _tile = null;
            Color = null;
            _rend.sprite = null;

            Destroy(gameObject);
            //TODO: Add To Pool
        }

        public void PerformSwapTo(Tile tile, float swapDuration)
        {
            transform.DOMove(tile.Position, swapDuration);
        }
    }
}