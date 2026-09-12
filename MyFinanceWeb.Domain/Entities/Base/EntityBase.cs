using System.ComponentModel.DataAnnotations;

namespace MyFinanceWeb.Domain.Entities.Base;

public abstract class EntityBase
{
    [Key]
    public int Id { get; set; }
}