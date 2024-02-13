namespace Crudite.Types;

public interface IBaseModel<TId> where TId : notnull {
    TId Id { get; }
}