start cmd /k "cd ChatDataServer&&dotnet run"
timeout /T 3
start cmd /k "cd ChatServer&&dotnet run"
timeout /T 3
start cmd /k "cd MLServer&&dotnet run"
timeout /T 3
start cmd /k "cd ApiServer&&dotnet run"
timeout /T 3
start cmd /k "cd WebViewServer&&dotnet run"
