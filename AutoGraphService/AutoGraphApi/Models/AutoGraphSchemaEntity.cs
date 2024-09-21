using System.ComponentModel.DataAnnotations.Schema;

namespace AutoGraphApi.Models;

[Table("auto_graph_schemas")]
public record AutoGraphSchemaEntity(
    [property: Column("schema_id")] string SchemaId,
    [property: Column("schema_name")] string SchemaName
) : AbstractEntity;