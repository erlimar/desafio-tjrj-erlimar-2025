import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LivrosComponent } from './livros.component';
import { provideHttpClient } from '@angular/common/http';

describe('LivrosComponent', () => {
  let component: LivrosComponent;
  let fixture: ComponentFixture<LivrosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LivrosComponent],
      providers: [
        provideHttpClient()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LivrosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
