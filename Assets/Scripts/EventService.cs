public class EventService
{
    private static EventService instance;

    public static EventService Instance
    {
        get
        {
            if(instance == null)
                instance = new EventService();

            return instance;
        }
    }

    public EventController OnTransactionCompleted { get; private set; }

    public EventController<ItemScriptableObject, ItemContext, int> OnTransactionPerformed { get; private set; }

    public EventService()
    {
        OnTransactionCompleted = new EventController();
        OnTransactionPerformed = new EventController<ItemScriptableObject, ItemContext, int>();
    }
}