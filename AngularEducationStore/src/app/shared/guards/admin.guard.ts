import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { DataService } from 'src/app/shared/services/data.service';

@Injectable({
    providedIn: 'root'
})
export class AdminGuard implements CanActivate {

    constructor(private dataService: DataService, private router: Router) { }

    canActivate() {
        let role = this.dataService.getLocalStorage('userRole');

        if (role === 'admin') {
            return true;
        }

        this.router.navigate(['/']);
        
        return false;
    }
}
