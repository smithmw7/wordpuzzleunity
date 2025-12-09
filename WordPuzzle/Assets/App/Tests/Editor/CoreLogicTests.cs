using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WordPuzzle.Game.Model;

namespace Tests
{
    public class CoreLogicTests
    {
        [Test]
        public void WordDictionary_Loads_And_Validates()
        {
            // Requires "words" TextAsset in Resources. 
            // In EditMode, Resources.Load might fail if not in the right context, but usually works if asset database is refreshed.
            // For strict unit testing without Unity Scene, we might mock it, but here we test integration.
            
            WordDictionary.Instance.Initialize();
            Assert.IsTrue(WordDictionary.Instance.IsInitialized);
            Assert.IsTrue(WordDictionary.Instance.IsWordValid("hello"));
            Assert.IsFalse(WordDictionary.Instance.IsWordValid("zzzzz"));
        }

        [Test]
        public void GameRules_CalculatePossibleMoves()
        {
            WordDictionary.Instance.Initialize();
            
            string currentWord = "PLATE";
            // Target: SLATE. We need 'S'.
            var rackTiles = new List<TileData> { new TileData("1", "S"), new TileData("2", "X") };
            
            var moves = GameRules.CalculatePossibleMoves(currentWord, rackTiles);
            
            Assert.IsTrue(moves.ContainsKey(1)); // 1 tile used
            Assert.Contains("SLATE", moves[1]);
        }

        [Test]
        public void LevelGenerator_GeneratesValidLevel()
        {
            WordDictionary.Instance.Initialize();
            var level = LevelGenerator.GenerateLevel(5);
            
            Assert.IsNotNull(level);
            Assert.AreEqual(5, level.startWord.Length);
            Assert.AreEqual(5, level.endWord.Length);
            Assert.AreEqual(5, level.rackTiles.Count);
            Assert.IsTrue(level.solution.Count > 0);
        }
    }
}
