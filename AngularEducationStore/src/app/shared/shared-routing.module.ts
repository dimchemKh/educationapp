import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SignInComponent } from 'src/app/account/sign-in/sign-in.component';
import { from } from 'rxjs';

export const routes: Routes = [
    { path: 'signIn', component: SignInComponent }
];

@NgModule({
    imports: [],
    exports: []
})
export class SharedRoutingModule {}
