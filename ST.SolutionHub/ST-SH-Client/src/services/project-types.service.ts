import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { ProjectTypeViewModel } from 'models/project-types/projectTypeViewModel';
import { ProjectTypeAddModel } from 'models/project-types/projectTypeAddModel';
import { ProjectTypeEditModel } from 'models/project-types/projectTypeEditModel';

@Injectable({
  providedIn: 'root'
})
export class ProjectTypesService {

  constructor(private httpClient: HttpClient) { }

  public getProjectTypes(): Observable<ProjectTypeViewModel[]> {
    return this.httpClient.get<ProjectTypeViewModel[]>(`${environment.apiBaseUrl}/ProjectType/get`);
  }
  getById(id: number): Observable<ProjectTypeViewModel> {
    return this.httpClient.get<any>(`${environment.apiBaseUrl}/ProjectType/${id}`);
  }
  public post(model: ProjectTypeAddModel): Observable<string> {
    const formData = new FormData();
    formData.append('id', JSON.stringify(model.id));
    formData.append('name', model.name);
    formData.append('description', model.description);
    formData.append('html', '');
    formData.append('attachment', model.attachment, model.name + '_' + model.attachment.name);
    return this.httpClient.post<any>(`${environment.apiBaseUrl}/ProjectType/post`, formData);
  }
  public put(model: ProjectTypeEditModel): Observable<string> {
    const formData = new FormData();
    formData.append('id', JSON.stringify(model.id));
    formData.append('name', model.name);
    formData.append('description', model.description);
    formData.append('html', model.html);
    formData.append('isAttachmentRemoved', JSON.stringify(model.isAttachmentRemoved));
    if (model.attachment) {
      formData.append('attachment', model.attachment, model.name + '_' + model.attachment.name);
    }
    return this.httpClient.put<any>(`${environment.apiBaseUrl}/ProjectType/put`, formData);
  }
}
