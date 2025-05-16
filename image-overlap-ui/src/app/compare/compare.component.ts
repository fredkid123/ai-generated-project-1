import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
	selector: 'app-compare',
	templateUrl: './compare.component.html',
	styleUrls: ['./compare.component.css']
})
export class CompareComponent {
	results: any[] = [];
	instanceId: string = this.generateInstanceId();

	constructor(private http: HttpClient) {}

	compare(): void {
		if (!this.instanceId) {
			alert('Instance ID ausente!');
			return;
		}

		this.http.post<any[]>(`/compare/${this.instanceId}`, {}).subscribe(data => {
			this.results = data;
		});
	}

	private generateInstanceId(): string {
		return Math.random().toString(36).substring(2, 10);
	}
}