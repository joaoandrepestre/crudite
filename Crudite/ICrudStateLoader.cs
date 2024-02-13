using Crudite.Types;

namespace Crudite;

public interface ICrudStateLoader<TRequest, TLoaded> {
    Task<IEnumerable<LoadedItems<TRequest, TLoaded>>> Load(IEnumerable<TRequest> loadRequests);
}