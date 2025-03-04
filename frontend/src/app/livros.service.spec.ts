import { TestBed } from '@angular/core/testing';

import { LivrosService } from './livros.service';
import { provideHttpClient } from '@angular/common/http';

describe('LivrosService', () => {
  let service: LivrosService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient()
      ]
    });
    service = TestBed.inject(LivrosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
