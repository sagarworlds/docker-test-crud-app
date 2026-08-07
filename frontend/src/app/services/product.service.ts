import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateProductDto, Product, UpdateProductDto } from '../models/product.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProductService {
  // Dev (Docker Compose): '' + '/api/products' → relative → Nginx proxy
  // Prod (Render):        'https://crud-api-xxxx.onrender.com' + '/api/products' → direct CORS call
  private readonly apiUrl = `${environment.apiUrl}/api/products`;

  constructor(private readonly http: HttpClient) {}

  getAll(search?: string): Observable<Product[]> {
    let params = new HttpParams();
    if (search?.trim()) {
      params = params.set('search', search.trim());
    }
    return this.http.get<Product[]>(this.apiUrl, { params });
  }

  getById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateProductDto): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, dto);
  }

  update(id: number, dto: UpdateProductDto): Observable<Product> {
    return this.http.put<Product>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
