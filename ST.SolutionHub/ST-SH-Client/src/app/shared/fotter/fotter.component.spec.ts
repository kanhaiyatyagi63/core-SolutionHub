/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FotterComponent } from './fotter.component';

describe('FotterComponent', () => {
  let component: FotterComponent;
  let fixture: ComponentFixture<FotterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FotterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FotterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
