import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UpdateTableService {
  public isRefreshed: Subject<boolean> = new Subject<boolean>();

  constructor() { }

  triggerRefresh() {
    this.isRefreshed.next(true);
  }
}
