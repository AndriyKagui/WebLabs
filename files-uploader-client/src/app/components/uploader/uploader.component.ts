import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ExtentionsConstants } from 'src/app/common/constants/constants';
import { UpdateTableService } from 'src/app/services/update-table.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-uploader',
  templateUrl: './uploader.component.html',
  styleUrls: ['./uploader.component.scss']
})
export class UploaderComponent implements OnInit {
  constants = ExtentionsConstants.allowebExtentions;
  @Output() public onUploadFinished = new EventEmitter();
  formGroup = new FormGroup({
    file: new FormControl('', Validators.required)
  });

  constructor(private http: HttpClient, private refreshService: UpdateTableService) { }

  ngOnInit() {
  }

  async submit() {
    const formData = new FormData();
    let file = this.formGroup.get('file').value;
    formData.append('file', file);
    if (file.size <= ExtentionsConstants.size * 1024 * 1024) {
      if ((ExtentionsConstants.allowebExtentions.includes(file.type) || ExtentionsConstants.allowebExtentions.length === 0)) {
        await this.http.post(environment.apiurl, formData).toPromise()
          .then(res => {
            console.log(res);
            alert("File uploaded.");

            this.refreshService.triggerRefresh();
          })
          .catch(error => {
            console.error(error);
            alert("Error uploading file");
          });
      }
      else {
        console.error("File type unsupported!");
        alert("Unsupporrted file type");
      }
    }
    else{
      console.error("File is too large");
      alert("File is too large")
    }
  }
}
