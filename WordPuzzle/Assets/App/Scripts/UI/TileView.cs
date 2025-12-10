using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using WordPuzzle.Game.Model;
using WordPuzzle.Core;

namespace WordPuzzle.UI
{
    public class TileView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("UI References")]
        public TextMeshProUGUI charText;
        public GameObject backContainer;
        public CanvasGroup canvasGroup;

        public TileData Data { get; private set; }
        private Transform _parentContainer;
        private Canvas _canvas;
        private Transform _dragStartParent;

        public void Initialize(TileData data, Transform container, Canvas canvas)
        {
            Data = data;
            charText.text = data.charValue;
            _parentContainer = container;
            _canvas = canvas;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            EventManager.TriggerEvent("TileDragBegin", this);
            
            // Unparent to render on top
            _dragStartParent = transform.parent;
            _parentContainer = transform.parent;
            transform.SetParent(_canvas.transform, true);
            canvasGroup.blocksRaycasts = false; // Allow raycast to pass through to slots
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Move with mouse/finger
            // Using logic for Screen Space Overlay or Camera
            if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            EventManager.TriggerEvent("TileDragEnd", this);

            // If no one reparented this tile (still on canvas), snap back to rack.
            if (transform.parent == _canvas.transform)
            {
                _parentContainer = _dragStartParent;
                ReturnToParent();
            }
        }

        public void ReturnToParent()
        {
            transform.SetParent(_parentContainer, false);
            transform.localPosition = Vector3.zero;
        }

        public void SetParentContainer(Transform newParent)
        {
            _parentContainer = newParent;
            ReturnToParent();
        }
    }
}
