/**
 * VyapSetu API - Angular Integration Reference
 * Copy this file into your Angular project (e.g. src/app/core/config/api-endpoints.ts)
 * and update apiBaseUrl in environment.ts for each deployment target.
 */

export const API_BASE_URL = 'http://localhost:5000';

export const ApiEndpoints = {
  products: {
    getAll: `${API_BASE_URL}/api/products`,
    getById: (id: number) => `${API_BASE_URL}/api/products/${id}`,
    search: (term: string) => `${API_BASE_URL}/api/products/search?term=${encodeURIComponent(term)}`,
    create: `${API_BASE_URL}/api/products`,
    update: (id: number) => `${API_BASE_URL}/api/products/${id}`,
    delete: (id: number) => `${API_BASE_URL}/api/products/${id}`,
  },
  customers: {
    getAll: `${API_BASE_URL}/api/customers`,
    getById: (id: number) => `${API_BASE_URL}/api/customers/${id}`,
    getByPhone: (phoneNumber: string) => `${API_BASE_URL}/api/customers/phone/${phoneNumber}`,
    create: `${API_BASE_URL}/api/customers`,
    update: (phoneNumber: string) => `${API_BASE_URL}/api/customers/${phoneNumber}`,
    delete: (phoneNumber: string) => `${API_BASE_URL}/api/customers/${phoneNumber}`,
  },
  bills: {
    create: `${API_BASE_URL}/api/bills`,
    getAll: `${API_BASE_URL}/api/bills`,
    getById: (id: number) => `${API_BASE_URL}/api/bills/${id}`,
    getByCustomer: (customerId: number) => `${API_BASE_URL}/api/bills/customer/${customerId}`,
  },
} as const;

/** Example Angular environment.ts */
export const environmentExample = {
  production: false,
  apiBaseUrl: 'http://localhost:5000',
};

/** Example Angular HttpClient service usage */
export const angularServiceExample = `
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly baseUrl = \`\${environment.apiBaseUrl}/api/products\`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse<Product[]>> {
    return this.http.get<ApiResponse<Product[]>>(this.baseUrl);
  }

  search(term: string): Observable<ApiResponse<Product[]>> {
    return this.http.get<ApiResponse<Product[]>>(\`\${this.baseUrl}/search?term=\${encodeURIComponent(term)}\`);
  }

  create(product: CreateProduct): Observable<ApiResponse<Product>> {
    return this.http.post<ApiResponse<Product>>(this.baseUrl, product);
  }
}

export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data?: T;
  errors?: string[];
}
`;
