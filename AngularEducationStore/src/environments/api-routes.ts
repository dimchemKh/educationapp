import { Injectable } from '@angular/core';

@Injectable()

export class ApiRoutes {
    private protocolHttp = 'http:/localhost:52196/';
    private protocolHttps = 'https:/localhost:44326/';

    public readonly accountRoute = this.protocolHttps + 'api/account/';
    public readonly authorsRoute = this.protocolHttps + 'api/author/';
    public readonly printingEditionRoute = this.protocolHttps + 'api/printingEdition/';
    public readonly userRoute = this.protocolHttps + 'api/user/';
    public readonly orderRoute = this.protocolHttps + 'api/order/';
}
