# Image Overlap App

Aplicação web para comparar imagens entre dois conjuntos (grupo A e grupo B) e identificar pares com "overlap" de padrão.

---

## 🧱 Tecnologias

- **Backend:** ASP.NET Core 8 Web API (com Serilog)
- **Frontend:** Angular 17 (com proxy para backend)
- **Log:** Console + Arquivo com rotação diária e por tamanho

---

## 📁 Estrutura de Pastas

```
/ImageOverlapApp        -> Backend .NET
  /Controllers
  /Models
  /Services
  /Logs/ImageOverlapApp
  Program.cs
  appsettings.json
  Directory.Build.props

/image-overlap-ui       -> Frontend Angular
  /src/app
    /upload
    /compare
  angular.json
  proxy.conf.json
  run-local.bat
```

---

## 🛠️ Backend

- `POST /upload/groupA` e `groupB`: upload via `multipart/form-data`
- `POST /compare`: lógica mock de comparação com base no prefixo do nome de arquivo
- Logs são gravados em:
  - Console
  - Arquivos em `log/ImageOverlapApp/log-<data>.txt`

---

## 🖥️ Frontend

### Componentes:
- **upload.component:** upload dos grupos A e B
- **compare.component:** dispara a comparação e exibe resultados

### Proxy:
`proxy.conf.json` redireciona chamadas para `https://localhost:61119`

### Script de execução:
```bat
@echo off
cd /d %~dp0

npm install
if %ERRORLEVEL% NEQ 0 (
  echo [ERRO] Falha ao instalar dependências.
  pause
  exit /b 1
)

if not exist node_modules\@angular-devkit\build-angular (
  echo [INFO] Instalando build-angular...
  npm install @angular-devkit/build-angular@17.3.12 --save-dev
)

npm run start
```

---

## ⚠️ Por que esse script?

A versão básica:
```bat
call npm install
call ng serve
```

...pode falhar silenciosamente se:
- `npm install` der erro
- `node_modules/` estiver incompleto
- faltar `@angular-devkit/build-angular`

Esse script defensivo garante que a aplicação sempre rode corretamente, mesmo após clonar ou reinstalar.

---

## ✅ Como rodar o projeto

1. **Backend**
```bash
cd ImageOverlapApp
dotnet run
```

2. **Frontend**
```bash
cd image-overlap-ui
run-local.bat
```

---

Desenvolvido com foco em robustez e separação clara entre frontend e backend.