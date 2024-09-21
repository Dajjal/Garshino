using Ardalis.Specification;
using AutoGraphApi.Models;

namespace AutoGraphApi.Services;

public sealed class GenericService<TEntity>(
    IRepositoryBase<TEntity> repository
) : IGenericService<TEntity>
    where TEntity : AbstractEntity
{
    public async Task<List<TEntity>> ListAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            // Получаем все записи из репозитория
            var entities = await repository.ListAsync(cancellationToken);
            // Преобразуем записи в модели для клиента
            return entities;
        }
        catch (Exception ex)
        {
            // Логируем ошибку и выбрасываем исключение
            throw new Exception("Ошибка при получении списка всех элементов.");
        }
    }

    public async Task<List<TEntity>> ListAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default
    ) 
    {
        try
        {
            // Получаем записи из репозитория по спецификации
            var entities = await repository.ListAsync(specification, cancellationToken);
            // Преобразуем записи в модели для клиента
            return entities;
        }
        catch (Exception ex)
        {
            // Логируем ошибку и выбрасываем исключение
            throw new Exception("Ошибка при получении списка элементов по спецификации.");
        }
    }

    public async Task<TEntity> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            // Находим запись в репозитории по идентификатору
            var entity = await repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new KeyNotFoundException($"Запись с ID: {id} не найдена в базе данных.");

            // Преобразуем запись в модель для клиента
            return entity;
        }
        catch (Exception ex)
        {
            // Логируем ошибку и выбрасываем исключение
            throw new Exception($"Ошибка при получении записи с ID: {id}.");
        }
    }

    public async Task<TEntity> AddAsync(
        TEntity dto,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(dto);
        try
        {
            return await repository.AddAsync(dto, cancellationToken);
        }
        catch (Exception ex)
        {
            // Логируем ошибку и выбрасываем исключение
            throw new Exception("Ошибка при добавлении новой записи.");
        }
    }

    public async Task<TEntity> UpdateAsync(
        Guid id,
        TEntity dto,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(dto);
        try
        {
            // Находим запись в репозитории по идентификатору
            var entity = await repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new KeyNotFoundException($"Запись с ID: {id} не найдена в базе данных.");

            // Проверяем, что ID в DTO совпадает с переданным ID
            if (dto is AbstractEntity adm && !Equals(adm.Id, id))
                throw new InvalidOperationException("ID в DTO не совпадает с переданным ID.");

            // Обновляем сущность и сохраняем изменения
            await repository.UpdateAsync(dto, cancellationToken);
            return dto;
        }
        catch (Exception ex)
        {
            // Логируем ошибку и выбрасываем исключение
            throw new Exception($"Ошибка при обновлении записи с ID: {id}.");
        }
    }

    public async Task<TEntity> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Находим запись в репозитории по идентификатору
            var entity = await repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new KeyNotFoundException($"Запись с ID: {id} не найдена в базе данных.");

            // Помечаем запись как удалённую и сохраняем изменения
            await repository.DeleteAsync(entity, cancellationToken);
            return entity;
        }
        catch (Exception ex)
        {
            // Логируем ошибку и выбрасываем исключение
            throw new Exception($"Ошибка при удалении записи с ID: {id}.");
        }
    }
}