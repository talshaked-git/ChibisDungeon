/// <summary>
/// IItemContainer Interface Documentation
/// 
/// The IItemContainer interface defines a common structure for managing items within a container. 
/// This interface can be implemented by different classes to create a variety of containers, 
/// such as inventory systems, chests, or storage units, in game development or other similar applications.
/// </summary>
public interface IItemContainer
{
    /// <summary>
    /// Retrieves the count of items with the specified item ID in the container.
    /// </summary>
    /// <param name="itemID">The string ID of the item for which to retrieve the count.</param>
    /// <returns>Returns the count of items with the specified item ID in the container.</returns>
    int ItemCount(string itemID);

    /// <summary>
    /// Checks if the container contains the specified item.
    /// </summary>
    /// <param name="item">The Item object to search for in the container.</param>
    /// <returns>Returns true if the item is found in the container, otherwise false.</returns>
    bool ContainsItem(Item item);

    /// <summary>
    /// Removes an item from the container based on its item ID.
    /// </summary>
    /// <param name="itemID">The string ID of the item to remove from the container.</param>
    /// <returns>Returns the removed Item object if found and successfully removed, otherwise null.</returns>
    Item RemoveItem(string itemID);

    /// <summary>
    /// Removes the specified item from the container.
    /// </summary>
    /// <param name="item">The Item object to remove from the container.</param>
    /// <returns>Returns true if the item was successfully removed from the container, otherwise false.</returns>
    bool RemoveItem(Item item);

    /// <summary>
    /// Adds the specified item to the container.
    /// </summary>
    /// <param name="item">The Item object to add to the container.</param>
    /// <returns>Returns true if the item was successfully added to the container, otherwise false.</returns>
    bool AddItem(Item item);

    /// <summary>
    /// Checks if the specified item and amount can be added to the container.
    /// </summary>
    /// <param name="item">The Item object to be added to the container.</param>
    /// <param name="amount">The optional int amount of the item to be added (default is 1).</param>
    /// <returns>Returns true if the item and specified amount can be added to the container, otherwise false.</returns>
    bool CanAddItem(Item item, int amount = 1);

    /// <summary>
    /// Removes all items from the container.
    /// </summary>
    void Clear();
}
