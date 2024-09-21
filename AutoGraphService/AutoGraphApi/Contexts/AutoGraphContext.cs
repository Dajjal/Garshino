using AutoGraphApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoGraphApi.Contexts;

public class AutoGraphContext(DbContextOptions<AutoGraphContext> options) : DbContext(options)
{
    public DbSet<AutoGraphUserEntity> AutoGraphUserEntities { get; init; }
    public DbSet<AutoGraphDriverEntity> AutoGraphDriverEntities { get; init; }
    public DbSet<AutoGraphMachinesEntity> AutoGraphMachinesEntities { get; init; }
    public DbSet<AutoGraphSchemaEntity> AutoGraphSchemaEntities { get; init; }
}