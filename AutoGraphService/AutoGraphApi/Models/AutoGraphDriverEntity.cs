using System.ComponentModel.DataAnnotations.Schema;

namespace AutoGraphApi.Models;

[Table("auto_graph_drivers")]
public record AutoGraphDriverEntity(
    [property: Column("driver_id")] Guid DriverId,
    [property: Column("driver_string_id")] string DriverStringId,
    [property: Column("driver_full_name")] string DriverFullName
) : AbstractEntity;