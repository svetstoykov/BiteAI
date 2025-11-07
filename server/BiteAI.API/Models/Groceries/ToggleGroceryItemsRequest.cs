namespace BiteAI.API.Models.Groceries;

public class ToggleGroceryItemsRequest
{
    public List<Guid> ItemIds { get; set; } = new();
}