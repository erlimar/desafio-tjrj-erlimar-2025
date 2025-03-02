param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName)

"Criando uma nova migração de banco..." | Write-Host -ForegroundColor Blue
Push-Location .\backend
    dotnet ef migrations add $MigrationName -s .\src\DesafioTjRjErlimar.WebApi -p .\src\DesafioTjRjErlimar.DatabaseAdapter
Pop-Location

