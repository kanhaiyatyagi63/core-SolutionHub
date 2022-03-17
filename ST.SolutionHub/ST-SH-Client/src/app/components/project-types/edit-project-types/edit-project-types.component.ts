import { ProjectTypeViewModel } from 'models/project-types/projectTypeViewModel';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ProjectTypeEditModel } from 'models/project-types/projectTypeEditModel';
import { ProjectTypesService } from 'services/project-types.service';

@Component({
  selector: 'app-edit-project-types',
  templateUrl: './edit-project-types.component.html',
  styleUrls: ['./edit-project-types.component.scss']
})
export class EditProjectTypesComponent implements OnInit {
  @ViewChild('fileInput')
  myInputVariable: ElementRef;
  form: FormGroup;
  id: number;
  model: ProjectTypeViewModel;
  isAttachmentDivVisible: boolean;
  isAttachment = false;
  projectTypeEditModel: ProjectTypeEditModel;
  constructor(private router: Router, private route: ActivatedRoute,
    private projectTypesService: ProjectTypesService) {
    this.projectTypeEditModel = new ProjectTypeEditModel();
    this.route.params.subscribe(x => this.id = Number(x.id));
  }
  loadDefaultFormData(): void {
    this.projectTypesService.getById(this.id).subscribe((data: ProjectTypeViewModel) => {
      if (data) {
        debugger;
        this.model = data;
        this.form.controls.name.setValue(data.name);
        this.form.controls.description.setValue(data.description);
        this.isAttachmentDivVisible = data.html ? true : false;
      }
    }, (error: any) => {
      console.log("not able to load the project type by id");
    });
  }

  ngOnInit() {
    this.form = new FormGroup({
      name: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      attachment: new FormControl('')
    });
    this.loadDefaultFormData();
  }
  onFileSelected(event: any): void {
    const fileList = event.target.files as FileList;
    if (!fileList || fileList.length === 0) {
      this.form.controls.attachment.setValue(null);
      return;
    }
    const names = fileList[0].name.split(".")
    const ext = names[names.length - 1];
    if (ext == 'png' || ext == 'jpg' || ext == 'jpeg' || ext == 'img') {
      this.form.controls.attachment.setValue(fileList.item(0));
    }
  }

  removeAttachment(event): void {
    this.isAttachmentDivVisible = false;
  }
  fileInputReset(): void {
    this.myInputVariable.nativeElement.value = '';
  }
  onSubmit(): void {
    debugger;
    console.log(this.form);
    if (this.form.invalid)
      return;
    this.projectTypeEditModel.id = this.id;
    this.projectTypeEditModel.name = this.form.controls.name.value;
    this.projectTypeEditModel.description = this.form.controls.description.value;
    this.projectTypeEditModel.attachment = this.form.controls.attachment.value;
    this.projectTypeEditModel.html = this.model.html;
    this.projectTypeEditModel.isAttachmentRemoved = !this.isAttachmentDivVisible;
    if (!this.isAttachmentDivVisible && !this.projectTypeEditModel.attachment)
      return;
    this.projectTypesService.put(this.projectTypeEditModel).subscribe((data: string) => {
      console.log(data);
      console.log('successful');
    }, (error: any) => {
      console.log('failed');
      console.log(error);
    });

  }
  onCancel(): void { }

}
