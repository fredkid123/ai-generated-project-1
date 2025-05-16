import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class InstanceService {
	private readonly instanceId: string;

	constructor() {
		this.instanceId = Math.random().toString(36).substring(2, 10);
	}

	getInstanceId(): string {
		return this.instanceId;
	}
}