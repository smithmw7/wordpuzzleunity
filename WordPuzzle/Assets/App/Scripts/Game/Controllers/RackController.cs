using System.Collections.Generic;
using UnityEngine;
using WordPuzzle.Game.Model;
using WordPuzzle.UI;
using WordPuzzle.Core;

namespace WordPuzzle.Game.Controllers
{
    public class RackController : MonoBehaviour
    {
        [Header("Config")]
        public Transform rackContainer;
        public GameObject tilePrefab;
        public Canvas mainCanvas;

        private List<TileView> _spawnedTiles = new List<TileView>();

        private void OnEnable()
        {
            EventManager.StartListening("LevelLoaded", OnLevelLoaded);
            EventManager.StartListening("ReturnTileToRack", OnReturnTile);
        }

        private void OnDisable()
        {
            EventManager.StopListening("LevelLoaded", OnLevelLoaded);
            EventManager.StopListening("ReturnTileToRack", OnReturnTile);
        }

        private void OnLevelLoaded(object payload)
        {
            ClearRack();
            var level = (LevelData)payload;
            foreach (var tileData in level.rackTiles)
            {
                var tileObj = Instantiate(tilePrefab, rackContainer);
                var tileView = tileObj.GetComponent<TileView>();
                tileView.Initialize(tileData, rackContainer, mainCanvas);
                _spawnedTiles.Add(tileView);
            }
        }

        private void OnReturnTile(object payload)
        {
            var tile = (TileView)payload;
            tile.SetParentContainer(rackContainer);
        }

        public void ClearRack()
        {
            foreach (var t in _spawnedTiles)
            {
                if(t != null) Destroy(t.gameObject);
            }
            _spawnedTiles.Clear();
        }

        public void Shuffle()
        {
            // Unity UI Grid/Layout Group usually orders by sibling index.
            // We can just shuffle sibling indices.
            foreach (var t in _spawnedTiles)
            {
                t.transform.SetSiblingIndex(Random.Range(0, rackContainer.childCount));
            }
        }
    }
}
