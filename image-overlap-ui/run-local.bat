@echo off
cd /d %~dp0

echo [INFO] Executando 'npm install' para garantir dependências...
npm install
if %ERRORLEVEL% NEQ 0 (
	echo [ERRO] Falha ao instalar dependências. Abortando.
	pause
	exit /b 1
)

REM Verifica se Angular Material está instalado
IF NOT EXIST "node_modules\@angular\material" (
	echo [INFO] Instalando Angular Material...
	npm install @angular/material @angular/cdk @angular/animations
)

npm run start