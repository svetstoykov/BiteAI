using BiteAI.Services.Data.Entities.Base;

namespace BiteAI.Services.Data.Entities;

public class GroceryItem : BaseIdentifiableEntity
{
    public Guid GroceryListId { get; set; }
    public GroceryList GroceryList { get; set; } = null!;

    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public bool Checked { get; set; }
}
