import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: 'account', loadChildren: () => import('src/app/account/account.module').then(x => x.AccountModule) },
  { path: 'user', loadChildren: () => import('src/app/user/user.module').then(x => x.UserModule) },
  { path: 'printing-edition', loadChildren: () => import('src/app/printing-edition/printing-edition.module')
  .then(x => x.PrintingEditionModule) },
  { path: 'author', loadChildren: () => import('src/app/author/author.module').then(x => x.AuthorModule) },
  { path: '', loadChildren: () => import('src/app/printing-edition/printing-edition.module')
  .then(x => x.PrintingEditionModule) },
  { path: 'order', loadChildren: () => import('src/app/order/order.module').then(x => x.OrderModule) },
  { path: '**', redirectTo: 'not-found' },
  { path: 'not-found', loadChildren: () => import('src/app/not-found/not-found.module')
  .then(x => x.NotFoundModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { initialNavigation: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule {}

