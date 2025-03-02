"Restaurando dependências de frontend..." | Write-Host -ForegroundColor Blue
Push-Location .\frontend
    npm install
Pop-Location

"`nRestaurando dependências de backend..." | Write-Host -ForegroundColor Blue
Push-Location .\backend
    dotnet tool restore
    dotnet restore
Pop-Location
