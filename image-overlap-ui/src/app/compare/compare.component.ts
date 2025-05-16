import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
	selector: 'app-compare',
	templateUrl: './compare.component.html',
	styleUrls: ['./compare.component.css']
})
export class CompareComponent {
	results: any[] = [];
	instanceId: string = '';

	constructor(private http: HttpClient) {}

	compare(): void {
		if (!this.instanceId) {
			alert('Informe o ID da instância de comparação.');
			return;
		}

		this.http.post<any[]>(`/compare/${this.instanceId}`, {}).subscribe(data => {
			this.results = data;
		});
	}
}