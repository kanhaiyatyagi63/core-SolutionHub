import { ProjectModel } from './../models/project/projectModel';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private httpClient: HttpClient) { }

  getProjectById(id: number): Observable<ProjectModel> {
    return this.httpClient.get<any>(`${environment.apiBaseUrl}/project/${id}`);
  }
}
