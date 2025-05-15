import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-compare',
  templateUrl: './compare.component.html',
  styleUrls: ['./compare.component.css']
})
export class CompareComponent {
  results: any[] = [];

  constructor(private http: HttpClient) {}

  compare(): void {
    this.http.post<any[]>('/compare', {}).subscribe(data => {
      this.results = data;
    });
  }
}