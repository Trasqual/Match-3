using System;
using UnityEngine;

public class TouchControls : MonoBehaviour
{
    public static Action<Vector2> OnTouchDown;
    public static Action<Vector2Int> OnTouchDrag;
    public static Action OnTouchUp;

    private Vector2 _touchStart;
    private Vector2Int _swipeDirection;

    private void Update()
    {
#if !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _touchStart = touch.position;
                _swipeDirection = Vector2Int.zero;
                OnTouchDown?.Invoke(_touchStart);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                var moveDelta = touch.position - _touchStart;
                if (moveDelta.magnitude < 20) return;

                if (Mathf.Abs(moveDelta.x) > Mathf.Abs(moveDelta.y))
                {
                    if (moveDelta.x > 0)
                    {
                        _swipeDirection = new Vector2Int(1, 0);
                    }
                    else
                    {
                        _swipeDirection = new Vector2Int(-1, 0);
                    }
                }
                else
                {
                    if (moveDelta.y > 0)
                    {
                        _swipeDirection = new Vector2Int(0, 1);
                    }
                    else
                    {
                        _swipeDirection = new Vector2Int(0, -1);
                    }
                }

                OnTouchDrag?.Invoke(_swipeDirection);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _touchStart = Vector2.zero;
                _swipeDirection = Vector2Int.zero;
                OnTouchUp?.Invoke();
            }
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            _touchStart = Input.mousePosition;
            _swipeDirection = Vector2Int.zero;
            OnTouchDown?.Invoke(_touchStart);
        }
        else if (Input.GetMouseButton(0))
        {
            var moveDelta = Input.mousePosition - new Vector3(_touchStart.x, _touchStart.y, 0f);

            if (moveDelta.magnitude < 20) return;

            if (Mathf.Abs(moveDelta.x) > Mathf.Abs(moveDelta.y))
            {
                if (moveDelta.x > 0)
                {
                    _swipeDirection = new Vector2Int(1, 0);
                }
                else
                {
                    _swipeDirection = new Vector2Int(-1, 0);
                }
            }
            else
            {
                if (moveDelta.y > 0)
                {
                    _swipeDirection = new Vector2Int(0, 1);
                }
                else
                {
                    _swipeDirection = new Vector2Int(0, -1);
                }
            }

            OnTouchDrag?.Invoke(_swipeDirection);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _touchStart = Vector2.zero;
            _swipeDirection = Vector2Int.zero;
            OnTouchUp?.Invoke();
        }
#endif
    }
}
