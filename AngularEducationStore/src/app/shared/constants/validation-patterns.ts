import { Injectable } from '@angular/core';

@Injectable()

export class ValidationPatterns {
    public readonly emailPattern = /^[a-zA-Z]{1}[a-zA-Z0-9.\-_]*@[a-zA-Z]{1}[a-zA-Z.-]*[a-zA-Z]{1}[.][a-zA-Z]{2,4}$/;
    public readonly namePattern = /^[a-zA-Z]{3,}$/;
}
