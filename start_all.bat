start cmd /k dotnet run --project dotnetDataSide/dotnetDataSide.csproj
timeout /T 3
start cmd /k dotnet run --project dotnetPrivateChatApi/dotnetPrivateChatApi.csproj
timeout /T 3
start cmd /k dotnet run --project dotnetApi/dotnetApi.csproj
timeout /T 3
start cmd /k dotnet run --project dotnet/dotnet.csproj