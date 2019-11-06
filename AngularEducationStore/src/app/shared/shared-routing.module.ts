import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';
import { SignInComponent } from 'src/app/account/sign-in/sign-in.component';

export const routes: Routes = [
    { path: 'signIn', component: SignInComponent }
];

@NgModule({
    imports: [],
    exports: []
})
export class SharedRoutingModule {}
