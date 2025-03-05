using System.ComponentModel.DataAnnotations;

namespace BiteAI.Services.Entities.Base;

public abstract class BaseIdentifiableEntity
{
    [Key]
    public Guid Id { get; set; }
}