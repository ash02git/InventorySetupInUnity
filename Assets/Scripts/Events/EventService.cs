public class EventService
{
    public EventController<ItemScriptableObject> OnItemSelected { get; private set; } //when an item is selected either from shop or inventory

    public EventController<ItemScriptableObject> OnItemActionInitiated { get; private set; } //when a buy or sell action is initiated

    public EventController<ItemScriptableObject, int> OnItemCountSet { get; private set; } //when the number of items to be bought/sold is set

    public EventController <ItemScriptableObject> OnItemActionConfirmed { get; private set; } //when confirmation is given

    public EventController OnTransactionCompleted { get; private set; } //when transaction is completed

    public EventController<ItemScriptableObject, int> OnItemBought { get; private set; } //when item is bought 
    
    public EventController<ItemScriptableObject, int> OnItemSold { get; private set; } //when item is sold

    public EventService()
    {
        //events related to controlling popups
        OnItemSelected = new EventController<ItemScriptableObject>();
        OnItemActionInitiated = new EventController<ItemScriptableObject>();
        OnItemCountSet = new EventController<ItemScriptableObject, int>();
        OnItemActionConfirmed = new EventController<ItemScriptableObject>();
        OnTransactionCompleted = new EventController();

        //events related to transactions
        OnItemBought = new EventController<ItemScriptableObject, int>();
        OnItemSold = new EventController<ItemScriptableObject, int>();
    }
}