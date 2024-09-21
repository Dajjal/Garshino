using System.ComponentModel.DataAnnotations.Schema;

namespace AutoGraphApi.Models;

public abstract record AbstractEntity
{
    [Column("id")] public virtual Guid Id { get; set; }
}