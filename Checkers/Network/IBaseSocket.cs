namespace Checkers.Network;

public interface IBaseSocket
{
    public abstract Task ProcessMessage(string message);
}
