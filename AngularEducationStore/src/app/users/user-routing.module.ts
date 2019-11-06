import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuard } from '../shared/guards/auth.guard';
import { UsersAllComponent } from './users-all/users-all.component';
import { AdminGuard } from '../shared/guards/admin.guard';


export const routes: Routes = [
    { path: 'me', component: ProfileComponent, canActivate: [AuthGuard] },
    { path: 'all', component: UsersAllComponent, canActivate: [AdminGuard] }
];

@NgModule({
    imports: [],
    exports: []
})
export class UserRoutingModule {}
