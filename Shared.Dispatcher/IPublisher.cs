namespace Shared.Dispatcher;

public interface IPublisher
{
    ValueTask Publish<TNotification>(
        TNotification notification,
        CancellationToken cancellationToken = default)
        where TNotification : INotification;
}

public interface INotification;

public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    ValueTask Handle(TNotification notification, CancellationToken cancellationToken);
}