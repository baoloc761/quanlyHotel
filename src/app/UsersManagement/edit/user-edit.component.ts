import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { User } from '@app/_models';
import { AccountService } from '@app/_services';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { first } from 'rxjs/operators';
@Component({ templateUrl: 'user-edit.component.html', styleUrls: [ './user-edit.component.scss'] })
export class UserEditComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;
    error?: string;
    user: any = {};
    private subscription: Subscription | undefined

    constructor(
        private formBuilder: FormBuilder,
        private accountService: AccountService,
        private dialogRef: MatDialog,
        @Inject(MAT_DIALOG_DATA) private data: any,
        private snackBar: MatSnackBar,
        private translate: TranslateService
    ) {
        this.user = this.data
    }

    myOptionsTypeUSer: any[] = [];
    selected?: any = {};

    ngOnInit() {
        this.accountService.getRolesList().subscribe(data => {
            this.myOptionsTypeUSer = data.map((x: any) => {
                return {id: x.id, name: x.name}});
        });
        this.form = this.formBuilder.group({
            FirstName: [this.user?.firstName || '', Validators.required],
            LastName: [this.user?.lastName || '', Validators.required],
            UserName: [this.user?.userName || '', Validators.required],
            Email: [this.user?.email || '', Validators.required],
            Password: ['', [Validators.required, Validators.minLength(6)]],
            Id: this.user?.id,
            UserTypeUser: [ this.enCodeValue({
                id: this.user.userTypeId,
                name: this.user.userTypeName
            }) || null, Validators.required ]
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    enCodeValue(val: any) {
        if (_.isEmpty(val)) return ''
        return JSON.stringify(val)
    }


    deCode(hashedValue: any) {
        if (_.isEmpty(hashedValue)) return {}
        return JSON.parse(hashedValue)
    }

    openSnackBar(message: string, action: string) {
        this.snackBar.open(message, action, {
          duration: 2000,
        });
    }

    onSubmit() {
        this.submitted = true;
        this.error = '';

        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        const newUser = {...this.form.value, UserTypeUser: [this.deCode(this.f.UserTypeUser.value)]}
        this.subscription = this.accountService.updateUser(newUser).subscribe(
            (data) => {
                if (data) {
                    const msg = this.translate.instant('ActionEntityResult', { entityName: this.translate.instant('Account'), actionName: this.translate.instant('Edit'), result:  this.translate.instant('Success') })
                    this.openSnackBar(msg, this.translate.instant('Success'))
                    this.dialogRef.closeAll()
                }
            },
            (error) => {
                this.error = error;
                this.loading = false;
                const msg = this.translate.instant('ActionEntityResultWithReason', { entityName: this.translate.instant('Account'), actionName: this.translate.instant('Edit'), result:  this.translate.instant('Failed'), reason: error })
                this.openSnackBar(msg, this.translate.instant('Failed'))
            }
        );
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    cancel() {
        this.dialogRef.closeAll()
    }
}