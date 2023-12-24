namespace Marina.UI.Infrastructure;

public class DatabaseManager
{
    private INotification _notification;

    public DatabaseManager(INotification notification)
    {
        _notification = notification;
    }

    public void Add()
    {
        _notification.Send("","");
    }

}


