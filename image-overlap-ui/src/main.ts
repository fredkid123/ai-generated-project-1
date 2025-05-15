import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';

console.log('[Angular] main.ts carregado');

platformBrowserDynamic().bootstrapModule(AppModule)
  .then(() => console.log('[Angular] AppModule bootstrapado com sucesso'))
  .catch(err => console.error('[Angular] Erro durante bootstrap:', err));