import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
	selector: 'app-upload',
	templateUrl: './upload.component.html',
	styleUrls: ['./upload.component.css']
})
export class UploadComponent {
	filesA: File[] = [];
	filesB: File[] = [];

	constructor(private http: HttpClient) {}

	onFileChange(event: any, group: string): void {
		const files = Array.from(event.target.files) as File[];
		if (group === 'groupA') {
			this.filesA = files;
		} else {
			this.filesB = files;
		}
	}

	upload(group: string): void {
		const formData = new FormData();
		const files = group === 'groupA' ? this.filesA : this.filesB;
		for (const file of files) {
			formData.append('files', file);
		}
		this.http.post(`/upload/${group}`, formData).subscribe(() => {
			alert(`${group} uploaded`);
		});
	}
}