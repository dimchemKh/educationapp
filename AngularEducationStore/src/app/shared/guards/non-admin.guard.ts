import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { DataService } from 'src/app/shared/services/data.service';

@Injectable({
    providedIn: 'root'
})
export class NonAdminGuard implements CanActivate {

    constructor(private dataService: DataService, private router: Router) { }

    canActivate() {
        if (this.dataService.getLocalStorage('userRole') !== 'admin') {
            return true;
        }
        this.router.navigate(['/printing-edition/manager']);
        return false;
    }
}
