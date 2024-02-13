using System.Collections;
using Crudite.Types;

namespace Crudite;

public class LoadableCrudState<TId, T, TRequest, TLoaded> : CrudState<TId, T>
    where TId : notnull
    where T : IBaseModel<TId> {
    
    private readonly ICrudStateLoader<TRequest, TLoaded> _loader;
    private readonly IDtoConverter<TRequest, TLoaded, T> _dtoConverter;

    protected virtual Task PreLoad() {
        return Task.CompletedTask;
    }

    protected virtual Task PostLoad() {
        return Task.CompletedTask;
    }

    public LoadableCrudState(ICrudStateLoader<TRequest, TLoaded> loader, IDtoConverter<TRequest, TLoaded, T> dtoConverter) {
        _loader = loader;
        _dtoConverter = dtoConverter;
    }

    public async Task Load(IEnumerable<TRequest> loadRequests) {
        await PreLoad();
        await DoLoad(loadRequests);
        await PostLoad();
    }

    protected async Task DoLoad(IEnumerable<TRequest> loadRequests) {
        var res  = await _loader.Load(loadRequests);
        foreach (var (req, dtos) in res) {
            await CreateLoaded(req, dtos);
        }
    }
    
    protected virtual Task<IEnumerable<T>> CreateLoaded(TRequest req, IEnumerable<TLoaded> dtos) {
        var values = dtos.Select(dto => _dtoConverter.ConvertDto(req, dto));
        return Create(values);
    }
}