using Ardalis.Specification.EntityFrameworkCore;
using AutoGraphApi.Contexts;
using AutoGraphApi.Models;

namespace AutoGraphApi.Repositories;

public class GenericRepository<TEntity>(AutoGraphContext dbContext) : RepositoryBase<TEntity>(dbContext)
    where TEntity : AbstractEntity;