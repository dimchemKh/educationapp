import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AccountService } from 'src/app/shared/services/account.service';
import { DataService } from '../services/data.service';

@Injectable({
    providedIn: 'root'
})
export class AdminGuard implements CanActivate {

    constructor(private dataService: DataService, private router: Router) { }

    canActivate() {
        if (this.dataService.getLocalStorage('userRole') === 'admin') {
            return true;
        }
        this.router.navigate(['/']);
        return false;
    }
}