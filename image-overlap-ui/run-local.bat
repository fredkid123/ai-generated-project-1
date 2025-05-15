@echo off
cd /d %~dp0

echo [INFO] Executando 'npm install' para garantir dependências...
call npm install

if exist node_modules\@angular-devkit\build-angular (
	echo [INFO] Dependência build-angular já instalada.
) else (
	echo [INFO] Instalando dependência crítica: @angular-devkit/build-angular
	call npm install @angular-devkit/build-angular@17.3.12 --save-dev
)

echo [INFO] Iniciando servidor Angular com 'npm run start'...
call npm run start

echo [INFO] Script finalizado.
pause
