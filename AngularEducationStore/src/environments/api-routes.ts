import { Injectable } from '@angular/core';

@Injectable()

export class ApiRoutes {
    public readonly accountRoute = 'http://localhost:52196/api/account/';
    public readonly authorsRoute = 'http://localhost:52196/api/author/';
    public readonly printingEditionRoute = 'http://localhost:52196/api/printingEdition/';
    public readonly userRoute = 'http://localhost:52196/api/user/';
}
