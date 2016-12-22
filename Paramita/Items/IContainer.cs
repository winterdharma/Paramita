namespace Paramita.Items
{
    /* This is the interface for any class that acts as a container
     * of Items. 
     * An IContainer is free to store the items it contains as
     * is appropriate to itself. It may also impose limitations to
     * its storage and evaluate them when transactions like Add and Remove
     * are requested.
     */
    public interface IContainer
    {
        /*
         * RemoveItem() removes @item from the IContainer
         * Returns that Item to the caller is successful
         * or null if the remove attempt failed for some reason.
         */ 
        Item RemoveItem(Item item);

        /*
         * AddItem() attempts to add an @item to the IContainer.
         * A bool is returned to the caller indicated success or failure.
         */
        bool AddItem(Item item);

        /*
         * InspectItems() will return an array of the IContainer's Items.
         */
        Item[] InspectItems();
    }
}
