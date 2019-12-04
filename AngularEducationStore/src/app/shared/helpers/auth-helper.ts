import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CartService } from 'src/app/shared/services';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ApiRoutes } from 'src/environments/api-routes';
import { AuthModel } from 'src/app/shared/models/auth/auth.model';
import * as jwt_decode from 'jwt-decode';
import { LocalDatabase } from '@ngx-pwa/local-storage';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
    providedIn: 'root'
})
export class AuthHelper {

    private readonly _userDataKey: string = 'USER_KEY';
    private readonly _cookiesKey: string = 'REMEMBER_KEY';

    private authSource: BehaviorSubject<boolean>;
    private currentUserData: AuthModel;
    private userImageSubject: BehaviorSubject<string>;

    constructor(
        private http: HttpClient,
        private router: Router,
        private cartService: CartService,
        private apiRoutes: ApiRoutes,
        private storage: LocalDatabase,
        private cookies: CookieService
    ) {
        this.authSource = new BehaviorSubject<boolean>(false);
        this.userImageSubject = new BehaviorSubject<string>(null);
        this.authSource.next(this.isAuth());
        this.storage.get(this._userDataKey).subscribe((data) => {
            this.currentUserData = data;
        });
    }

    get userImageSubject$(): Observable<string> {
        return this.userImageSubject.asObservable();
    }

    setUserImageSubject(value: string): void {
        this.userImageSubject.next(value);
    }

    getAuthNavStatus(): Observable<boolean> {
        return this.authSource.asObservable();
    }

    public isRemember(cheked: boolean): void {
        let date = new Date();

        if (!cheked) {
            this.cookies.set(this._cookiesKey, '', date.setHours(date.getHours() + 12));
        }

        if (cheked) {
            this.cookies.set(this._cookiesKey, '', date.setMonth(date.getMonth() + 2));
        }
    }

    public isAuth(): boolean {
        if (this.storage.get(this._userDataKey) !== null) {
            return true;
        }

        return false;
    }

    private getDecodedAccessToken(token: string) {
        try {
            return jwt_decode(token);
        } catch (error) {
            return null;
        }
    }

    public getUserNameFromToken(): string {
        let userName = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';

        try {
            let token = this.getDecodedAccessToken(this.getAccessToken());

            return token[userName];
        } catch (error) {
            return null;
        }
    }

    public getUserImageFromToken(): string {
        let image = 'image';

        try {
            let token = this.getDecodedAccessToken(this.getAccessToken());

            return token[image];
        } catch (error) {
            return null;
        }
    }

    public getUserRoleFromToken(): string {
        let role = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

        try {
            let token = this.getDecodedAccessToken(this.getAccessToken());

            return token[role];
        } catch (error) {
            return null;
        }
    }

    private getAccessToken(): string {
        if (this.currentUserData) {
            return this.currentUserData.accessToken;
        }
        return undefined;
    }

    login(data: AuthModel): void {
        this.currentUserData = data;

        this.storage.set(this._userDataKey, data).subscribe();

        this.authSource.next(this.isAuth());

        this.router.navigate(['/printing-edition/manager']);
    }

    logout(): void {
        this.storage.clear().subscribe();

        this.cookies.deleteAll('/');

        this.authSource.next(false);

        this.cartService.nextCartSource([]);

        this.http.get(this.apiRoutes.accountRoute + 'signOut').subscribe();

        this.router.navigate(['/account/signIn']);
    }
}
