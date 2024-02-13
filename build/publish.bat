del ..\publish\crudite /F /Q
del ..\publish\crudite.types /F /Q

dotnet pack ..\Crudite.Types\Crudite.Types.csproj -c Release --output "../publish/crudite.types"
dotnet pack ..\Crudite\Crudite.csproj -c Release --output "../publish/crudite"

nuget init ..\publish C:\LocalNuget\
