@echo off
cd /d %~dp0

echo [INFO] Executando 'npm install' para garantir dependências...
npm install
if %ERRORLEVEL% NEQ 0 (
	echo [ERRO] Falha ao instalar dependências. Abortando.
	pause
	exit /b 1
)

npm run start
