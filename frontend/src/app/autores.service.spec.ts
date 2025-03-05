import { TestBed } from '@angular/core/testing';

import { AutoresService } from './autores.service';
import { provideHttpClient } from '@angular/common/http';

describe('AutoresService', () => {
  let service: AutoresService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient()
      ]
    });
    service = TestBed.inject(AutoresService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
