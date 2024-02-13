using System.Collections;
using Crudite.Types;

namespace Crudite; 

public class CrudState<TId, T> : ICrudState<TId, T>
    where TId : notnull 
    where T : IBaseModel<TId> {
    
    protected readonly Dictionary<TId, T> Cache = new();

    protected readonly object Locker = new();
    
    protected virtual T Create(T value) {
        T? current;
        lock (Locker) {
            if (Cache.TryGetValue(value.Id, out current)) {
                return current;
            }
            Cache.Add(value.Id, value);
        }
        if (current != null)
            Console.WriteLine($"{GetType().Name}: Duplicate Id {value.Id}");
        return value;
    }
    
    public virtual Task<IEnumerable<T>> Create(IEnumerable<T> values) {
        var created = new List<T>();
        foreach (var v in values)
            created.Add(Create(v));
        IEnumerable<T> ret = created;
        return Task.FromResult(ret);
    }

    public virtual Task<T?> Read(TId id) {
        T? value;
        lock (Locker) {
            if (!Cache.TryGetValue(id, out value))
                return Task.FromResult<T?>(default);
        }
        return Task.FromResult<T?>(value);
    }

    protected virtual T? Update(T value) {
        T? current;
        lock (Locker) {
            if (!Cache.TryGetValue(value.Id, out current)) {
                Console.WriteLine($"{GetType().Name}: Could not find Id {value.Id} on update");
                return default;
            }
            Cache[value.Id] = value;
        }
        return value;
    }

    public virtual Task<IEnumerable<T>> Update(IEnumerable<T> dtos) {
        var values = new List<T>();
        foreach (var dto in dtos) {
            var updated = Update(dto);
            if (updated is not null)
                values.Add(updated);
        }
        IEnumerable<T> ret = values;
        return Task.FromResult(ret);
    }

    public virtual async Task<T?> Delete(TId id) {
        var current = await Read(id);
        if (current is null) return default;
        lock (Locker) {
            Cache.Remove(id);
        }
        return current;
    }
}