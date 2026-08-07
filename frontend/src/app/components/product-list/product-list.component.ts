import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  computed,
  signal
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { Product } from '../../models/product.model';
import { ProductService } from '../../services/product.service';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [RouterLink, CurrencyPipe, DatePipe, ConfirmDialogComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductListComponent implements OnInit {

  // ── State signals ─────────────────────────────────────────
  readonly products      = signal<Product[]>([]);
  readonly searchQuery   = signal('');
  readonly loading       = signal(true);
  readonly error         = signal<string | null>(null);
  readonly showConfirm   = signal(false);
  readonly productToDelete = signal<Product | null>(null);

  // ── Derived (computed) ────────────────────────────────────
  readonly filteredProducts = computed(() => {
    const q = this.searchQuery().toLowerCase().trim();
    if (!q) return this.products();
    return this.products().filter(
      (p) =>
        p.name.toLowerCase().includes(q) ||
        p.category?.toLowerCase().includes(q) ||
        p.description?.toLowerCase().includes(q)
    );
  });

  readonly deleteMessage = computed(() => {
    const p = this.productToDelete();
    return p
      ? `Are you sure you want to delete "${p.name}"? This action cannot be undone.`
      : '';
  });

  constructor(private readonly productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading.set(true);
    this.error.set(null);
    this.productService.getAll().subscribe({
      next:  (products) => { this.products.set(products); this.loading.set(false); },
      error: ()         => { this.error.set('Failed to load products. Please try again.'); this.loading.set(false); }
    });
  }

  onSearchChange(event: Event): void {
    this.searchQuery.set((event.target as HTMLInputElement).value);
  }

  clearSearch(): void { this.searchQuery.set(''); }

  confirmDelete(product: Product): void {
    this.productToDelete.set(product);
    this.showConfirm.set(true);
  }

  onConfirmDelete(): void {
    const product = this.productToDelete();
    if (!product) return;
    this.productService.delete(product.id).subscribe({
      next:  () => {
        this.products.update((list) => list.filter((p) => p.id !== product.id));
        this.showConfirm.set(false);
        this.productToDelete.set(null);
      },
      error: () => {
        this.error.set('Failed to delete product. Please try again.');
        this.showConfirm.set(false);
      }
    });
  }

  onCancelDelete(): void {
    this.showConfirm.set(false);
    this.productToDelete.set(null);
  }
}
