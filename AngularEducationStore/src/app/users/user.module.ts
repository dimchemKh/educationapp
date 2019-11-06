import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/users/user-routing.module';
import { ProfileComponent } from './profile/profile.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatSortModule,
    MatTableModule,
    MatPaginatorModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatDialogModule} from '@angular/material';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ValidationPatterns } from '../shared/constants/validation-patterns';
import { UsersAllComponent } from './users-all/users-all.component';
import { UserParametrs } from '../shared/constants/user-parametrs';
import { UserEditDialogComponent } from './users-all/user-edit-dialog/user-edit-dialog.component';
import { UserRemoveDialogComponent } from './users-all/user-remove-dialog/user-remove-dialog.component';

@NgModule({
    declarations: [
        ProfileComponent,
        UsersAllComponent,
        UserEditDialogComponent,
        UserRemoveDialogComponent
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        FontAwesomeModule,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        MatButtonModule,
        ReactiveFormsModule,
        FormsModule,
        MatTooltipModule,
        MatChipsModule,
        MatProgressSpinnerModule,
        MatSortModule,
        MatTableModule,
        MatPaginatorModule,
        MatSelectModule,
        MatSlideToggleModule,
        MatDialogModule
    ],
    exports: [],
    providers: [],
    entryComponents: [UserEditDialogComponent, UserRemoveDialogComponent]
})
export class UserModule {}
