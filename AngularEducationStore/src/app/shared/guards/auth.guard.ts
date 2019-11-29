import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthHelper } from '../helpers/auth-helper';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {

    constructor(private authHelper: AuthHelper, private router: Router) { }

    canActivate() {
        if (this.authHelper.isAuth()) {
            return true;
        }

        this.router.navigate(['/account/signIn']);
        
        return false;
    }
}
