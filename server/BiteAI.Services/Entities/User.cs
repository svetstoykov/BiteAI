using BiteAI.Services.Interfaces.Base;

namespace BiteAI.Services.Entities;

public class User : IEntity
{
    public string Id { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public string Email { get; set; } = null!;
}