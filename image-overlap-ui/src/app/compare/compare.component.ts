import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { InstanceService } from '../instance.service';

@Component({
	selector: 'app-compare',
	templateUrl: './compare.component.html',
	styleUrls: ['./compare.component.css']
})
export class CompareComponent {
	results: any[] = [];

	constructor(private http: HttpClient, private instanceService: InstanceService) {}

	compare(): void {
		const instanceId = this.instanceService.getInstanceId();
		if (!instanceId) {
			alert('Instance ID ausente!');
			return;
		}

		this.http.post<any[]>(`/compare/${instanceId}`, {}).subscribe(data => {
			this.results = data;
		});
	}
}