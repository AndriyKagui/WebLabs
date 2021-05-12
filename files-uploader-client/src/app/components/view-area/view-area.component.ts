import { FileViewModel } from './../../common/models/FileViewModel';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UpdateTableService } from 'src/app/services/update-table.service';

@Component({
  selector: 'app-view-area',
  templateUrl: './view-area.component.html',
  styleUrls: ['./view-area.component.scss']
})
export class ViewAreaComponent implements OnInit {
  files: FileViewModel[];
  data: Map<string, FileViewModel[]>;
  displayedColumns: string[] = ['name', 'size', 'uploadDate'];

  constructor(private http: HttpClient, refreshService: UpdateTableService) {
    refreshService.isRefreshed.subscribe(async x => {
      await this.getFilesData();
      this.getDataPairs();
    })
  }

  async ngOnInit() {
    await this.getFilesData();
    this.getDataPairs();
  }

  getDataPairs() {
    this.data = new Map<string, FileViewModel[]>();
    this.files.forEach(file => {
      if (this.data.has(file.extention)) {
        this.data.get(file.extention).push(file);
      }
      else {
        this.data.set(file.extention, []);
        this.data.get(file.extention).push(file);
      }
    });
  }

  async getFilesData() {
    await this.http.get<FileViewModel[]>(environment.apiurl).toPromise().then(res => {
      this.files = res;
    })
      .catch(error => {
        console.log(error);
      });
  }

}
