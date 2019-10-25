import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SignInComponent } from 'src/app/account/sign-in/sign-in.component';
import { from } from 'rxjs';
import { GetPrintingEditionComponent } from '../printing-edition/get-printing-edition/get-printing-edition.component';

export const routes: Routes = [
    { path: 'signIn', component: SignInComponent },
    { path: '', component: GetPrintingEditionComponent }
];

@NgModule({
    imports: [],
    exports: []
})
export class SharedRoutingModule {}
