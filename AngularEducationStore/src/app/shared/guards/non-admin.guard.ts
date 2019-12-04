import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class NonAdminGuard implements CanActivate {

    constructor(private router: Router) { }

    canActivate() {
        // if (this.dataService.getLocalStorage('userRole') !== 'admin') {
        //     return true;
        // }

        this.router.navigate(['/printing-edition/manager']);
        return true;

        return false;
    }
}
