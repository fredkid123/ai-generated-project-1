import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { UploadComponent } from './uploadty/upload.component';
import { CompareComponent } from './compare/compare.component';
import { ErrorInterceptor } from './error.interceptor';

@NgModule({
	declarations: [AppComponent, UploadComponent, CompareComponent],
	imports: [BrowserModule, FormsModule, HttpClientModule],
	providers: [
		{ provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
	],
	bootstrap: [AppComponent]
})
export class AppModule {}