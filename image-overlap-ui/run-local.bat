@echo off
cd /d %~dp0

echo [INFO] Executando 'npm install' para garantir dependências...
npm install
if %ERRORLEVEL% NEQ 0 (
	echo [ERRO] Falha ao instalar dependências. Abortando.
	pause
	exit /b 1
)

REM Verificação defensiva para pacote essencial
if not exist node_modules\@angular-devkit\build-angular (
	echo [INFO] Instalando pacote crítico ausente: @angular-devkit/build-angular
	npm install @angular-devkit/build-angular@17.3.12 --save-dev
)

echo [INFO] Iniciando servidor Angular...
npm run start
