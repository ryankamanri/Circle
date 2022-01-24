start cmd /k dotnet run --project ChatDataServer/ChatDataServer.csproj
timeout /T 3
start cmd /k dotnet run --project ChatServer/ChatServer.csproj
timeout /T 3
start cmd /k dotnet run --project ApiServer/ApiServer.csproj
timeout /T 3
start cmd /k dotnet run --project WebViewServer/WebViewServer.csproj