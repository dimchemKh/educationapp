import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/user/user-routing.module';
import { ProfileComponent } from 'src/app/user/profile/profile.component';
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
    MatDialogModule } from '@angular/material';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { UsersAllComponent } from 'src/app/user/users-all/users-all.component';
import { UserEditDialogComponent } from 'src/app/user/users-all/user-edit-dialog/user-edit-dialog.component';

@NgModule({
    declarations: [
        ProfileComponent,
        UsersAllComponent,
        UserEditDialogComponent
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
    entryComponents: [UserEditDialogComponent]
})
export class UserModule {}
