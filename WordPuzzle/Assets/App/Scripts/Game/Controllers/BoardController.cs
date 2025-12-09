using System.Collections.Generic;
using UnityEngine;
using WordPuzzle.Game.Model;
using WordPuzzle.UI;
using WordPuzzle.Core;
using System.Linq;

namespace WordPuzzle.Game.Controllers
{
    public class BoardController : MonoBehaviour
    {
        [Header("Config")]
        public Transform slotsContainer;
        public GameObject slotPrefab;
        
        private List<SlotView> _slots = new List<SlotView>();
        private LevelData _currentLevel;

        private void OnEnable()
        {
            EventManager.StartListening("LevelLoaded", OnLevelLoaded);
            EventManager.StartListening("TileDroppedOnSlot", OnTileDropped);
        }

        private void OnDisable()
        {
            EventManager.StopListening("LevelLoaded", OnLevelLoaded);
            EventManager.StopListening("TileDroppedOnSlot", OnTileDropped);
        }

        private void OnLevelLoaded(object payload)
        {
            ClearBoard();
            _currentLevel = (LevelData)payload;

            // Spawn Slots
            for (int i = 0; i < _currentLevel.startWord.Length; i++)
            {
                var slotObj = Instantiate(slotPrefab, slotsContainer);
                var slotView = slotObj.GetComponent<SlotView>();
                slotView.Initialize(i, _currentLevel.startWord[i].ToString());
                _slots.Add(slotView);
            }
        }

        private void OnTileDropped(object payload)
        {
            var dropData = (DropPayload)payload;
            var targetSlot = _slots[dropData.slotIndex];

            // If slot has a tile, return it to rack (or swap?)
            if (targetSlot.HasTile)
            {
               var existingTile = targetSlot.tileContainer.GetComponentInChildren<TileView>();
               if (existingTile != null)
               {
                   EventManager.TriggerEvent("ReturnTileToRack", existingTile);
               }
            }

            // Move new tile to slot
            targetSlot.SetTile(dropData.tile);
        }

        public void SubmitWord()
        {
            string candidateWord = "";
            bool incomplete = false;

            foreach (var slot in _slots)
            {
                 var tile = slot.tileContainer.GetComponentInChildren<TileView>();
                 if (tile != null)
                 {
                     candidateWord += tile.Data.charValue;
                 }
                 else
                 {
                     candidateWord += slot.lockedCharText.text; // Use locked char
                 }
            }

            if (GameRules.IsWordValid(candidateWord))
            {
                Debug.Log("Valid Word: " + candidateWord);
                // Lock in tiles
                // Check Win
                // For now, assume simplified win for any valid word not in history 
                // But simplified: just Level Won if no tiles left in rack?
                // Logic needs to update "Locked Chars" to new word.
                
                foreach(var slot in _slots)
                {
                    var tile = slot.tileContainer.GetComponentInChildren<TileView>();
                    if (tile != null)
                    {
                        slot.Initialize(slot.Index, tile.Data.charValue); // Update locked char
                        Destroy(tile.gameObject); // Consumed
                    }
                }
                
                // Check Rack Empty
                var rackHasTiles = FindObjectOfType<RackController>().transform.childCount > 0; // Quick dirty check or use event
                // Actually RackController should check count.
                // Let's rely on event.
                
                // If we consumed tiles, we should check if level is complete.
                // In React app, win condition is rackTiles.length === 0.
                
                // We need to know how many tiles are left.
                // A better way: BoardController shouldn't know about Rack internal state ideally, 
                // but checking GameObject hierarchy is a valid Unity pattern for simple games.
                
                // Let's Trigger "WordSubmitted" event and let GameManager decide?
                EventManager.TriggerEvent("WordSubmitted", candidateWord);
            }
            else
            {
                Debug.Log("Invalid Word");
                // Bounce back all staged tiles
                 foreach(var slot in _slots)
                {
                    var tile = slot.tileContainer.GetComponentInChildren<TileView>();
                    if (tile != null)
                    {
                        EventManager.TriggerEvent("ReturnTileToRack", tile);
                        slot.ClearTile();
                    }
                }
            }
        }

        private void ClearBoard()
        {
            foreach(var s in _slots)
            {
                if(s != null) Destroy(s.gameObject);
            }
            _slots.Clear();
        }
    }
}
