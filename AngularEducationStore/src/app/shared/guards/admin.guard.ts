import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class AdminGuard implements CanActivate {

    constructor(private router: Router) { }

    canActivate() {
        // let role = this.dataService.getLocalStorage('userRole');

        // if (role === 'admin') {
        //     return true;
        // }

        this.router.navigate(['/']);
        return true;

        return false;
    }
}
