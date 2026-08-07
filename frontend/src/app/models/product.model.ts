export interface Product {
  id: number;
  name: string;
  description: string | null;
  price: number;
  category: string | null;
  createdAt: string;
  updatedAt: string | null;
}

export interface CreateProductDto {
  name: string;
  description: string | null;
  price: number;
  category: string | null;
}

export type UpdateProductDto = CreateProductDto;
