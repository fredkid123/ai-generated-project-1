import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { InstanceService } from '../instance.service';

@Component({
	selector: 'app-upload',
	templateUrl: './upload.component.html',
	styleUrls: ['./upload.component.css']
})
export class UploadComponent {
	filesA: File[] = [];
	filesB: File[] = [];
	message: string = '';

	constructor(private http: HttpClient, private instanceService: InstanceService) {}

	onFileChange(event: any, group: string): void {
		const files = Array.from(event.target.files) as File[];
		if (group === 'groupA') {
			this.filesA = files;
		} else {
			this.filesB = files;
		}
	}

	upload(group: string): void {
		const instanceId = this.instanceService.getInstanceId();
		if (!instanceId) {
			alert('Instance ID ausente!');
			return;
		}

		const formData = new FormData();
		const files = group === 'groupA' ? this.filesA : this.filesB;
		for (const file of files) {
			formData.append('files', file);
		}

		this.http.post(`/upload/${group}/${instanceId}`, formData).subscribe(() => {
			this.message = `Grupo ${group === 'groupA' ? 'A' : 'B'} enviado com sucesso.`;
			if (group === 'groupA') {
				this.filesA = [];
			} else {
				this.filesB = [];
			}
		});
	}
}