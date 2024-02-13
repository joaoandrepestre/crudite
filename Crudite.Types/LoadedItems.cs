using System.Collections;

namespace Crudite.Types; 

public class LoadedItems<TRequest, TLoaded> {
    public TRequest Request { get; set; }
    public IEnumerable<TLoaded> Items { get; set; }

    public void Deconstruct(out TRequest request, out IEnumerable<TLoaded> items) {
        request = Request;
        items = Items;
    }

    public static implicit operator LoadedItems<TRequest, TLoaded>((TRequest req, IEnumerable<TLoaded> items) t) => new() {
        Request = t.req,
        Items = t.items,
    };
}