# Gera senhas aleatórias para usuários de banco
$senhaSa = -join ((48..57+64..90+95+97..122) | Get-Random -Count 20 | ForEach-Object {[char]$_})
$senhaWebApi = -join ((48..57+64..90+95+97..122) | Get-Random -Count 20 | ForEach-Object {[char]$_})

# Cria o arquivo de variáveis de ambiente [infra/dev.env]
"Gerando arquivo infra/dev.env" | Write-Host
$devEnvFileContent = @"
MSSQL_SA_PASSWORD=${senhaSa}
ACCEPT_EULA=Y
MSSQL_PID=Express
"@

$devEnvFileContent | Out-File -FilePath infra/dev.env -Encoding utf8

# Cria o arquivo de inicialização do banco [infra/db_scripts/create_database.sql]
"Gerando arquivo infra/db_scripts/create_database.sql" | Write-Host
$dbInitFileContent = @"
USE [master];
GO

-- Cria usuário "desafio_tjrj_erlimar_db_user" para uso na aplicação
IF NOT EXISTS (SELECT * FROM sys.sql_logins WHERE name = 'desafio_tjrj_erlimar_db_user')
BEGIN
    CREATE LOGIN [desafio_tjrj_erlimar_db_user] WITH PASSWORD = '${senhaWebApi}', CHECK_POLICY = OFF;
    ALTER SERVER ROLE [sysadmin] ADD MEMBER [desafio_tjrj_erlimar_db_user];
END
GO

CREATE DATABASE [desafio_tjrj_erlimar_db]
GO
"@

$dbInitFileContent | Out-File -FilePath infra/db_scripts/create_database.sql -Encoding utf8

"Confira os arquivos [infra/dev.env] e [infra/db_scripts/create_database.sql] para obter as senhas dos usuários de banco" | Write-Warning
