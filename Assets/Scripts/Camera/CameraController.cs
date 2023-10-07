using GamePlay.Events;
using UnityEngine;

namespace GamePlay.CameraUtilities
{
    [DefaultExecutionOrder(-10)]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _cam;

        private void Awake()
        {
            EventManager.Instance.AddListener<BoardFilledInitallyEvent>(AdjustCamera);
        }

        private void AdjustCamera(object data)
        {
            var boardData = ((BoardFilledInitallyEvent)data).Data;

            var boardSize = boardData.Width;

            _cam.orthographicSize = boardSize + 1;
            //TODO check size for both dimensions.

            _cam.transform.position = new Vector3(boardData.Width / 2f - 0.5f, boardData.Height / 2f - 0.5f, -50f);
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener<BoardFilledInitallyEvent>(AdjustCamera);
        }
    }
}