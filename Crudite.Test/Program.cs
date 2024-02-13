using System.Collections;
using Crudite;
using Crudite.Types;

var loader = new Loader(new Fetcher());
var state = new State(loader, new Converter());
await state.Load(new[] { 0, 1, 2, 3});
var m = await state.Read(1);
Console.WriteLine(m?.Value);

var normalState = new CrudState<int, Model>();
await normalState.Create(new[] { new Model { Id = 1, Value = "jsnksdm" } });
m = await normalState.Read(1);
Console.WriteLine(m?.Value);

public class Model : IBaseModel<int> {
    public int Id { get; set;  }
    
    public string Value { get; set; }
}

public interface IFetcher {
    public Task<IEnumerable<string>> Fetch(int request);
}

public class Fetcher : IFetcher {
    public Task<IEnumerable<string>> Fetch(int request) {
        IEnumerable<string> ret = new[] { "asasas", "asdasd", "sdfsfsd" };
        return Task.FromResult(ret);
    }
}

public class Loader : ICrudStateLoader<int, string> {
    private IFetcher _fetcher;

    public Loader(IFetcher fetcher) {
        _fetcher = fetcher;
    }
    public async Task<IEnumerable<LoadedItems<int, string>>> Load(IEnumerable<int> loadRequests) {
        Console.WriteLine("Initial fetch...");
        var ret = new List<LoadedItems<int, string>>();
        foreach (var req in loadRequests) {
            var dtos = await Fetch(req);
            ret.Add((req, dtos));
        }
        Console.WriteLine("Initial fetch done.");
        return ret;
    }
    
    public async Task<IEnumerable<string>> Fetch(int request) {
        Console.WriteLine($"Fetching {typeof(string).Name} data for {request}...");
        var res = await _fetcher.Fetch(request);
        Console.WriteLine($"Fetching {typeof(string).Name} data for {request} done. Fetched {res.Count()} items.");
        return res ?? Enumerable.Empty<string>();
    }
}

public class Converter : IDtoConverter<int, string, Model> {
    private int _id = 0;

    public Model ConvertDto(int request, string dto) {
        var id = 3*request + (_id);
        _id = (_id+1) % 3;
        return new Model {
            Id = id,
            Value = dto,
        };
    }
}

public class State : LoadableCrudState<int, Model, int, string> {

    public State(Loader loader, Converter dtoConverter) : base(loader, dtoConverter) { }

    protected override async Task PreLoad() {
        await base.PreLoad();
        Console.WriteLine($"Loading state {GetType().Name}...");
    }

    protected override async Task PostLoad() {
        await base.PostLoad();
        Console.WriteLine($"Loading state {GetType().Name} done.");
    }

}