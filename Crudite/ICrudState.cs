using Crudite.Types;

namespace Crudite; 

public interface ICrudState<TId, T>
    where TId : notnull
    where T : IBaseModel<TId> {
    Task<IEnumerable<T>> Create(IEnumerable<T> dtos);
    Task<T?> Read(TId id);
    Task<IEnumerable<T>> Update(IEnumerable<T> dtos);
    Task<T?> Delete(TId id);
}