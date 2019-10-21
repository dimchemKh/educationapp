import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GetAuthorsComponent } from './get-authors.component';

describe('GetAuthorsComponent', () => {
  let component: GetAuthorsComponent;
  let fixture: ComponentFixture<GetAuthorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GetAuthorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GetAuthorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
