using GamePlay.Board;
using GamePlay.StateMachine;
using System.Collections.Generic;

namespace Main.Gameplay.Core
{
    public static class MatchFinder
    {
        private static List<Tile> processedTiles = new List<Tile>();

        private static int indexPointer;

        private static void AddMember(Tile tile)
        {
            if (!processedTiles.Contains(tile))
            {
                processedTiles.Add(tile);
                indexPointer++;
            }
        }

        private static void ClearBuffer()
        {
            processedTiles.Clear();
            indexPointer = 0;
        }

        public static bool FindMatches(this Tile tile, out List<Tile> tiles, int minMatches = 2)
        {
            ClearBuffer();

            tile.FindHorizontalMatches(minMatches);

            tile.FindVerticalMatches(minMatches);

            tiles = new List<Tile>(processedTiles);

            return tiles.Count >= minMatches;
        }

        private static void FindHorizontalMatches(this Tile tile, int minMatches = 2)
        {
            var foundMatches = 0;
            foundMatches += tile.FindMatchInDirection(Neighbour.Left, 1);

            foundMatches += tile.FindMatchInDirection(Neighbour.Right, 1);

            if (foundMatches > 0 && foundMatches < minMatches)
            {
                processedTiles.RemoveRange(indexPointer - 1, foundMatches);
                indexPointer -= foundMatches;
            }
        }

        private static void FindVerticalMatches(this Tile tile, int minMatches = 2)
        {
            var foundMatches = 0;
            foundMatches += tile.FindMatchInDirection(Neighbour.Up, 1);

            foundMatches += tile.FindMatchInDirection(Neighbour.Down, 1);

            if (foundMatches > 0 && foundMatches < minMatches)
            {
                processedTiles.RemoveRange(indexPointer - 1, foundMatches);
                indexPointer -= foundMatches;
            }
        }

        private static int FindMatchInDirection(this Tile tile, Neighbour direction, int minMatches = 2)
        {
            var counter = 0;
            while (tile.GetMatchingNeighbour(direction, out Tile foundTile))
            {
                AddMember(foundTile);
                tile = foundTile;
                counter++;
            }
            if (counter > 0 && counter < minMatches)
            {
                processedTiles.RemoveRange(indexPointer - 1, counter);
                indexPointer -= counter;
                return 0;
            }
            return counter;
        }

        public static bool HasMatchInDirection(this Tile tile, Neighbour direction, int minMatches = 3)
        {
            var foundMatches = 1;
            while (tile.GetMatchingNeighbour(direction, out Tile foundTile))
            {
                foundMatches++;
                tile = foundTile;
            }
            if (foundMatches >= minMatches)
            {
                return true;
            }
            return false;
        }

        private static bool GetMatchingNeighbour(this Tile tile, Neighbour direction, out Tile matchingTile)
        {
            var neighbour = tile.GetNeighbour(direction);
            if (neighbour)
            {
                if (tile.StateManager.CurrentState is TileHasDropState && neighbour.StateManager.CurrentState is TileHasDropState)
                {
                    if (tile.CurrentDrop != null && neighbour.CurrentDrop != null && neighbour.CurrentDrop.Color.GetType() == tile.CurrentDrop.Color.GetType())
                    {
                        matchingTile = neighbour;
                        return true;
                    }
                }
            }
            matchingTile = null;
            return false;
        }
    }
}