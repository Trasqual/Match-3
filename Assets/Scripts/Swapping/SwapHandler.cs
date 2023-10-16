using DG.Tweening;
using GamePlay.Board;
using GamePlay.Events;
using Main.Gameplay.Core;
using System.Collections.Generic;

public static class SwapHandler
{
    public static void TrySwap(Tile firstTile, Tile secondTile)
    {
        EventManager.Instance.TriggerEvent<SwapStartedEvent>();

        var firstDrop = firstTile.CurrentDrop;
        var secondDrop = secondTile.CurrentDrop;

        firstDrop.PerformSwapTo(secondTile, 0.5f);
        if (secondDrop != null)
            secondDrop.PerformSwapTo(firstTile, 0.5f);

        DOVirtual.DelayedCall(0.5f, () =>
        {
            firstTile.AcceptDropTemprorary(secondDrop);
            secondTile.AcceptDropTemprorary(firstDrop);

            var firstMatchedTiles = new List<Tile>();
            var secondMatchedTiles = new List<Tile>();

            var firstHasMatches = MatchFinder.FindMatches(firstTile, out firstMatchedTiles);
            var secondHasMatches = MatchFinder.FindMatches(secondTile, out secondMatchedTiles);

            if (firstHasMatches || secondHasMatches)
            {
                firstTile.AcceptDrop(secondDrop);
                secondTile.AcceptDrop(firstDrop);

                if (firstHasMatches)
                {
                    firstMatchedTiles.Add(firstTile);
                    foreach (var tile in firstMatchedTiles)
                    {
                        tile.PopDrop();
                    }
                }

                if (secondHasMatches)
                {
                    secondMatchedTiles.Add(secondTile);
                    foreach (var tile in secondMatchedTiles)
                    {
                        tile.PopDrop();
                    }
                }

                EventManager.Instance.TriggerEvent<SwapEndedEvent>();
            }
            else
            {
                firstDrop.PerformSwapTo(firstTile, 0.5f);
                if (secondDrop != null)
                    secondDrop?.PerformSwapTo(secondTile, 0.5f);
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    firstTile.AcceptDrop(firstDrop);
                    secondTile.AcceptDrop(secondDrop);
                    EventManager.Instance.TriggerEvent<SwapEndedEvent>();
                }).SetUpdate(false);
            }
        }).SetUpdate(false);
    }
}
