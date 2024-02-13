namespace Crudite; 

public interface IDtoConverter<in TRequest, in TDto, out T> {
    T ConvertDto(TRequest request, TDto dto);
}