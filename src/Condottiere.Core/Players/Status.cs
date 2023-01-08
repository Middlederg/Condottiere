namespace Condottiere.Core.Players;

public class Status
{
    private readonly bool isDefending;

    private bool hasPassed;
    private bool isWaiting;

    public Status(bool isDefending)
    {
        this.isDefending = isDefending;
        isWaiting = isDefending;
        hasPassed = false;
    }

    public void Pass()
    {
        if (!(isDefending && isWaiting))
            hasPassed = true;
    }

    public void Play()
    {
        if (isDefending)
            isWaiting = false;
    }

    public bool CanPlayMoreCards => !hasPassed;
}
