"Destruindo última migração de banco..." | Write-Host -ForegroundColor Red
Push-Location .\backend
    dotnet ef migrations remove -s .\src\DesafioTjRjErlimar.WebApi -p .\src\DesafioTjRjErlimar.DatabaseAdapter
Pop-Location

