import { ProjectTypesService } from './../../../../services/project-types.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ProjectTypeAddModel } from 'models/project-types/projectTypeAddModel';

@Component({
  selector: 'app-new-project-types',
  templateUrl: './new-project-types.component.html',
  styleUrls: ['./new-project-types.component.scss']
})
export class NewProjectTypesComponent implements OnInit {
  @ViewChild('fileInput')
  myInputVariable: ElementRef;
  form: FormGroup;
  isAttachmentDivVisible: false;
  projectTypeAddModel: ProjectTypeAddModel;
  constructor(private router: Router, private route: ActivatedRoute, private projectTypesService: ProjectTypesService) {
    this.projectTypeAddModel = new ProjectTypeAddModel();
  }

  ngOnInit() {
    this.form = new FormGroup({
      name: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      attachment: new FormControl('', Validators.required)
    });
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
    console.log(this.form);
    if (this.form.invalid)
      return;
    this.projectTypeAddModel.id = 0;
    this.projectTypeAddModel.name = this.form.controls.name.value;
    this.projectTypeAddModel.description = this.form.controls.description.value;
    this.projectTypeAddModel.attachment = this.form.controls.attachment.value;
    this.projectTypesService.post(this.projectTypeAddModel).subscribe((data: string) => {
      console.log(data);
      console.log('successful');
    }, (error: any) => {
      console.log('failed');
      console.log(error);
    });

  }
  onCancel(): void { }


}
