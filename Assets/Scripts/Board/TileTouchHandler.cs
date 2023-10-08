using UnityEngine;

namespace GamePlay.Board
{
    public class TileTouchHandler
    {
        private Tile _tile;
        private Camera _cam;

        private bool _isTouched;

        public TileTouchHandler(Tile tile)
        {
            _tile = tile;
            _cam = Camera.main;

            TouchControls.OnTouchDown += OnTouchDown;
            TouchControls.OnTouchDrag += OnTouchDrag;
            TouchControls.OnTouchUp += OnTouchUp;
        }

        private void OnTouchDown(Vector2 touchPosition)
        {
            if (_tile.CurrentDrop == null) return;
            _isTouched = IsTouched(touchPosition);
        }

        private void OnTouchDrag(Vector2Int touchDelta)
        {
            if (!_isTouched) return;

            _isTouched = false;

            Neighbour swapNeighbour = Neighbour.None;
            if (touchDelta.x == 1)
            {
                swapNeighbour = Neighbour.Right;
            }
            else if (touchDelta.x == -1)
            {
                swapNeighbour = Neighbour.Left;
            }
            else if (touchDelta.y == 1)
            {
                swapNeighbour = Neighbour.Up;
            }
            else if (touchDelta.y == -1)
            {
                swapNeighbour = Neighbour.Down;
            }
            _tile.TrySwap(swapNeighbour);
        }

        private void OnTouchUp()
        {
            if (!_isTouched) return;

            _isTouched = false;
        }

        private bool IsTouched(Vector2 touchPosition)
        {
            var touchPos = new Vector3(touchPosition.x, touchPosition.y, _tile.Position.z);
            var touchWorldPos = _cam.ScreenToWorldPoint(touchPos);

            return (touchWorldPos.x < _tile.Position.x + 0.5f &&
                touchWorldPos.x > _tile.Position.x - 0.5f &&
                touchWorldPos.y < _tile.Position.y + 0.5f &&
                touchWorldPos.y > _tile.Position.y - 0.5f);
        }
    }
}