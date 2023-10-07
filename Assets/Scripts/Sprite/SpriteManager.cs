using GamePlay.Colors;
using UnityEngine;
using UnityEngine.U2D;
using Utilities;

namespace GamePlay.Visuals
{
    [DefaultExecutionOrder(-50)]
    public class SpriteManager : Singleton<SpriteManager>
    {
        [SerializeField] private SpriteAtlas _dropAtlas;

        public Sprite GetDropSprite(IColor color)
        {
            var sprite = color switch
            {
                Red => _dropAtlas.GetSprite("RedDrop"),
                Green => _dropAtlas.GetSprite("GreenDrop"),
                Blue => _dropAtlas.GetSprite("BlueDrop"),
                Yellow => _dropAtlas.GetSprite("YellowDrop"),
                _ => null
            };

            return sprite;
        }
    }
}