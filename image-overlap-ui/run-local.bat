@echo off
cd /d %~dp0
if not exist node_modules (
	echo 🔁 Instalando dependências...
	call npm install
)
call npm run start
