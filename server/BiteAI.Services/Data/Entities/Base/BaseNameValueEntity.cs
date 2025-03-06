using System.ComponentModel.DataAnnotations;

namespace BiteAI.Services.Data.Entities.Base;

public abstract class BaseNameValueEntity<TValue, TName> where TValue : struct
{
    public BaseNameValueEntity(TValue value, TName name)
    {
        this.Value = value;
        this.Name = name;
    }
    
    [Required]
    public TValue Value { get; set; }

    [Required]
    public TName Name { get; set; }
}