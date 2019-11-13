import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from 'src/app/not-found/page/not-found.component';

export const routes: Routes = [
    { path: '', component: NotFoundComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class NotFoundRoutingModule { 

}
