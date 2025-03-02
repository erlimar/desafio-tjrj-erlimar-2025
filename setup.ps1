# Gera uma senha temporária para o banco de dados
$senhaTemporaria = -join ((48..57+64..90+95+97..122) | Get-Random -Count 20 | ForEach-Object {[char]$_})

# Cria o arquivo de variáveis de ambiente infra/dev.env
"Gerando arquivo infra/dev.env..." | Write-Host
$devEnvFileContent = @"
MSSQL_SA_PASSWORD=${senhaTemporaria}
ACCEPT_EULA=Y
MSSQL_PID=Express
"@

$devEnvFileContent | Out-File -FilePath infra/dev.env -Encoding utf8

"A senha do banco de dados pode ser obtida no arquivo infra/dev.env como MSSQL_SA_PASSWORD" | Write-Warning
