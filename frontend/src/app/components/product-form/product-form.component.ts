import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  signal
} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NgClass } from '@angular/common';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, NgClass],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductFormComponent implements OnInit {

  form!: FormGroup;

  readonly isEdit    = signal(false);
  readonly productId = signal<number | null>(null);
  readonly loading   = signal(false);
  readonly submitting= signal(false);
  readonly error     = signal<string | null>(null);

  constructor(
    private readonly fb:             FormBuilder,
    private readonly route:          ActivatedRoute,
    private readonly router:         Router,
    private readonly productService: ProductService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name:        ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.maxLength(500)]],
      price:       [null, [Validators.required, Validators.min(0)]],
      category:    ['', [Validators.maxLength(50)]]
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit.set(true);
      this.productId.set(Number(id));
      this.loadProduct(Number(id));
    }
  }

  private loadProduct(id: number): void {
    this.loading.set(true);
    this.productService.getById(id).subscribe({
      next: (product) => {
        this.form.patchValue({
          name:        product.name,
          description: product.description ?? '',
          price:       product.price,
          category:    product.category ?? ''
        });
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load product. Please go back and try again.');
        this.loading.set(false);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    this.error.set(null);

    const v = this.form.value;
    const dto = {
      name:        v.name,
      description: v.description || null,
      price:       Number(v.price),
      category:    v.category || null
    };

    const req$ = this.isEdit()
      ? this.productService.update(this.productId()!, dto)
      : this.productService.create(dto);

    req$.subscribe({
      next:  () => this.router.navigate(['/products']),
      error: () => {
        this.error.set('Failed to save product. Please check your inputs and try again.');
        this.submitting.set(false);
      }
    });
  }

  // Convenience getters used in the template
  get nameCtrl()        { return this.form.get('name')!; }
  get descriptionCtrl() { return this.form.get('description')!; }
  get priceCtrl()       { return this.form.get('price')!; }
  get categoryCtrl()    { return this.form.get('category')!; }
}
