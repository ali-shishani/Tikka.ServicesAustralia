import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LayoutRoutingModule } from './layout-routing.module';
import { HomeComponent } from './components/home/home.component';
import { MatButtonModule } from '@angular/material/button';


@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    // Import our Routes for this module
    LayoutRoutingModule,
    // Angular Material Imports
    MatButtonModule,
  ]
})
export class LayoutModule { }
