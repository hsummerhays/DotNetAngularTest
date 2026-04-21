import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { Product, ProductService } from './services/product';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent implements OnInit {
  title = 'Clean Architecture AWS App';
  products: Product[] = [];
  loading = true;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.productService.getProducts()
      .pipe(
        catchError(() => {
          return of([
            { id: 1, name: 'AWS Lambda Processor', description: 'Serverless compute service that runs code in response to events.', price: 19.99 },
            { id: 2, name: 'S3 Storage Vault', description: 'Highly scalable object storage service for any data.', price: 49.99 },
            { id: 3, name: 'RDS Managed Cluster', description: 'Relational Database Service that simplifies database setup.', price: 89.00 }
          ]);
        })
      )
      .subscribe(data => {
        this.products = data;
        this.loading = false;
      });
  }
}
