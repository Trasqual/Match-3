using DG.Tweening;
using GamePlay.Board;
using GamePlay.Events;

public static class SwapHandler
{
    public static void TrySwap(Tile firstTile, Tile secondTile)
    {
        EventManager.Instance.TriggerEvent<SwapStartedEvent>();

        var firstDrop = firstTile.CurrentDrop;
        var secondDrop = secondTile.CurrentDrop;

        firstDrop.PerformSwapTo(secondTile, 0.5f);
        secondDrop.PerformSwapTo(firstTile, 0.5f);

        DOVirtual.DelayedCall(0.5f, () =>
        {
            firstTile.AcceptDropTemproraryForSwap(secondDrop);
            secondTile.AcceptDropTemproraryForSwap(firstDrop);

            //if matched
            //firstTile.AcceptDrop(secondDrop);
            //secondTile.AcceptDrop(firstDrop);
            //EventManager.Instance.TriggerEvent<SwapEndedEvent>();
            //executegameloop
            //else
            firstDrop.PerformSwapTo(firstTile, 0.5f);
            secondDrop.PerformSwapTo(secondTile, 0.5f);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                firstTile.AcceptDrop(firstDrop);
                secondTile.AcceptDrop(secondDrop);
                EventManager.Instance.TriggerEvent<SwapEndedEvent>();
            });
        });
    }
}
