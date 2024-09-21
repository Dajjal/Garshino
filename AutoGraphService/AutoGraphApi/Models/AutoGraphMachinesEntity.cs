using System.ComponentModel.DataAnnotations.Schema;

namespace AutoGraphApi.Models;

[Table("auto_graph_machines")]
public record AutoGraphMachinesEntity(
    [property: Column("machine_id")] Guid MachineId,
    [property: Column("machine_name")] string MachineName,
    [property: Column("machine_reg_number")] string MachineRegNumber,
    [property: Column("parent_id")] Guid ParentId
) : AbstractEntity;