using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using WordPuzzle.Game.Model;
using WordPuzzle.Core;

namespace WordPuzzle.UI
{
    public class SlotView : MonoBehaviour, IDropHandler
    {
        [Header("UI References")]
        public TextMeshProUGUI lockedCharText;
        public GameObject backContainer;
        public Transform tileContainer; // Where the TileView sits

        public int Index { get; private set; }
        public bool IsLocked { get; private set; }
        public bool HasTile => tileContainer.childCount > 0;

        public void Initialize(int index, string charValue)
        {
            Index = index;
            lockedCharText.text = charValue;
            IsLocked = true; // Initially background
            lockedCharText.gameObject.SetActive(true);
        }

        public void OnDrop(PointerEventData eventData)
        {
            var tile = eventData.pointerDrag.GetComponent<TileView>();
            if (tile != null)
            {
                // Logic to accept drop
                // Notify BoardController via Event
                EventManager.TriggerEvent("TileDroppedOnSlot", new DropPayload { tile = tile, slotIndex = Index });
            }
        }

        public void SetTile(TileView tile)
        {
            if (tile == null) return;
            tile.SetParentContainer(tileContainer);
            lockedCharText.gameObject.SetActive(false); // Hide background char
        }

        public void ClearTile()
        {
            lockedCharText.gameObject.SetActive(true);
        }
    }

    public struct DropPayload
    {
        public TileView tile;
        public int slotIndex;
    }
}
