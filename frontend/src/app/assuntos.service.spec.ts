import { TestBed } from '@angular/core/testing';

import { AssuntosService } from './assuntos.service';
import { provideHttpClient } from '@angular/common/http';

describe('AssuntosService', () => {
  let service: AssuntosService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient()
      ]
    });
    service = TestBed.inject(AssuntosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
