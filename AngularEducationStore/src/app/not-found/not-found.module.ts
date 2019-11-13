import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { routes } from 'src/app/not-found/not-found-routing.module';
import { NotFoundComponent } from 'src/app/not-found/page/not-found.component';

@NgModule({
    imports: [
        RouterModule.forChild(routes),
    ],
    declarations: [NotFoundComponent],
    bootstrap: [],
})
export class NotFoundModule {

 }
