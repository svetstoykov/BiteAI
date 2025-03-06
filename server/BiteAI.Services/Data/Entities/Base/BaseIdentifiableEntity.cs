using System.ComponentModel.DataAnnotations;

namespace BiteAI.Services.Data.Entities.Base;

public abstract class BaseIdentifiableEntity
{
    [Key]
    public Guid Id { get; set; }
}