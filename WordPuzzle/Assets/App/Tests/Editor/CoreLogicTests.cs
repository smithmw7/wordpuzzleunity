using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            WordDictionary.Instance.Initialize();
            Assert.IsTrue(WordDictionary.Instance.IsInitialized);
            Assert.IsTrue(WordDictionary.Instance.IsWordValid("about"));
            Assert.IsTrue(WordDictionary.Instance.IsWordValid("abort"));
            Assert.IsFalse(WordDictionary.Instance.IsWordValid("zzzzz"));
        }

        [Test]
        public void GameRules_CalculatePossibleMoves()
        {
            WordDictionary.Instance.Initialize();
            
            string currentWord = "ABOUT";
            var puzzleRack = WordDictionary.Instance.CurrentPuzzle.rack;
            var rackTiles = puzzleRack
                .Select((c, idx) => new TileData(idx.ToString(), c.ToUpper()))
                .ToList();
            
            var moves = GameRules.CalculatePossibleMoves(currentWord, rackTiles);
            
            var allMoves = new List<string>();
            foreach (var entry in moves.Values)
            {
                allMoves.AddRange(entry);
            }

            Assert.Contains("SHOUT", allMoves);
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
            Assert.AreEqual(level.startWord, level.endWord);
            Assert.Greater(level.totalSolutions, 0);
            Assert.Greater(level.totalPaths, 0);
        }
    }
}
