import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AccountService } from 'src/app/shared/services/account.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {

    constructor(private accountService: AccountService, private router: Router) { }

    canActivate() {
        if (this.accountService.isAuth()) {
            return true;
        }

        this.router.navigate(['/account/signIn']);
        
        return false;
    }
}
