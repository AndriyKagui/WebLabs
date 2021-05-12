import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { UploaderComponent } from './uploader.component';
import { environment } from 'src/environments/environment';

describe('UploaderComponent', () => {
  let component: UploaderComponent;
  let fixture: ComponentFixture<UploaderComponent>;
  let httpTestingController: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [ UploaderComponent ]
    });
    httpTestingController = TestBed.get(HttpTestingController);
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UploaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should return too large error', () => {
    const http = TestBed.get(HttpTestingController);
    var size = "";
        for (var i = 0; i < 60*1024*1024; i++) {
          size += "a";
        }
    let file = new File([size], "Test", { type: 'text/html' });
    component.formGroup.setValue({'file': file});
    component.submit();
    http.expectNone(environment.apiurl);
  });

  it('should return unsupported type error', () => {
    component.constants.push('text/html');
    component.constants.push('text/plain');
    const http = TestBed.get(HttpTestingController);
    var size = "";
        for (var i = 0; i < 4*1024*1024; i++) {
          size += "a";
        }
    let file = new File([size], "Test", { type: 'application/json' });
    component.formGroup.setValue({'file': file});
    component.submit();
    http.expectNone(environment.apiurl);
    component.submit();
  });

  it('should return ok', () => {
    var size = "";
        for (var i = 0; i < 4*1024*1024; i++) {
          size += "a";
        }
    let file = new File([size], "Test", { type: 'text/html' });
    component.formGroup.setValue({'file': file});
    component.submit();
    let req = httpTestingController.expectOne(environment.apiurl);
    expect(req.request.method).toEqual('POST');
  });
});
