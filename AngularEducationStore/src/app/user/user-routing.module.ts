import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';
import { ProfileComponent } from 'src/app/user/profile/profile.component';
import { AuthGuard } from 'src/app/shared/guards/auth.guard';
import { UsersAllComponent } from 'src/app/user/users-all/users-all.component';
import { AdminGuard } from 'src/app/shared/guards/admin.guard';


export const routes: Routes = [
    { path: 'me', component: ProfileComponent, canActivate: [AuthGuard] },
    { path: 'all', component: UsersAllComponent, canActivate: [AdminGuard] }
];

@NgModule({
    imports: [],
    exports: []
})
export class UserRoutingModule {}
