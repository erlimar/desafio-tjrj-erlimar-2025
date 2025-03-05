import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssuntosComponent } from './assuntos.component';
import { provideHttpClient } from '@angular/common/http';

describe('AssuntosComponent', () => {
  let component: AssuntosComponent;
  let fixture: ComponentFixture<AssuntosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssuntosComponent],
      providers:[
        provideHttpClient()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssuntosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
