"Gerando script de banco..." | Write-Host -ForegroundColor Yellow
Push-Location .\backend
    dotnet ef migrations script --idempotent -s .\src\DesafioTjRjErlimar.WebApi -p .\src\DesafioTjRjErlimar.DatabaseAdapter -o ..\infra\db_scripts\database_apply.sql
Pop-Location
