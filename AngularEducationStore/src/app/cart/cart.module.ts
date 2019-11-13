import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartItemsComponent } from './cart-items/cart-items.component';
import {
  MatTableModule,
  MatSelectModule,
  MatButtonModule
} from '@angular/material';



@NgModule({
  declarations: [CartItemsComponent],
  imports: [
    CommonModule,
    MatTableModule,
    MatSelectModule,
    MatButtonModule
  ],
  entryComponents: [CartItemsComponent]
})
export class CartModule { }
