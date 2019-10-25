import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: 'account', loadChildren: () => import('src/app/account/account.module').then(x => x.AccountModule) },
  // tslint:disable-next-line: max-line-length
  { path: 'printingedition', loadChildren: () => import('src/app/printing-edition/printing-edition.module').then(x => x.PrintingEditionModule) },
  { path: 'author', loadChildren: () => import('src/app/author/author.module').then(x => x.AuthorModule) },
  { path: 'order', loadChildren: () => import('src/app/account/account.module').then(x => x.AccountModule) },
  { path: 'user', loadChildren: () => import('src/app/account/account.module').then(x => x.AccountModule) },
  { path: 'author', loadChildren: () => import('./author/author.module').then(m => m.AuthorModule) },
  // tslint:disable-next-line: max-line-length
  { path: 'account', loadChildren: () => import('src/app/shared/shared.module').then(m => m.SharedModule) },
  { path: '', loadChildren: () => import('src/app/printing-edition/printing-edition.module').then(x => x.PrintingEditionModule) }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}

