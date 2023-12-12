import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild} from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { FormBuilder, FormGroup, FormsModule, NgForm } from '@angular/forms';
import { AccountService } from '@app/_services';
import { MatDialog } from '@angular/material/dialog';
import { RegisterComponent } from '@app/LoginHotel/register.component';
import { TranslateService } from '@ngx-translate/core';
import { User } from '@app/_models';
import { UserEditComponent } from './edit/user-edit.component';

@Component({
  selector: 'users-list',
  templateUrl: './users.component.html',
  styleUrls: [ './users.component.scss' ]
})
export class UsersComponent implements OnInit {
  form!: FormGroup;
  users: any[] = [];
  user: any = JSON.parse(localStorage.getItem('user') || '{claims: []}')
  isEdit?: boolean

  constructor(private formBuilder: FormBuilder,
     private accountService: AccountService,
     private dialogRef: MatDialog, private translate: TranslateService,
     private router: Router) {
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
        keyword: ['', null]
    });
    
    this.accountService.getUsers(this.keyword || '').subscribe(data => {
      this.users = data;
    })
  }

  get keyword(): string {
    return this.form.get('keyword')?.value
  }

  search() {
    this.accountService.getUsers(this.keyword).subscribe(data => {
      this.users = data;
    })
  }

  handleUserInfo(user: any) {
    this.router.navigateByUrl('/account/user/' + user.id ) 
  }

  checkEdit(isEdit: boolean) {
    return isEdit
  }

  handleEditUser(user: any) {
    user.isEdit = this.isEdit = true
    this.dialogRef.open(UserEditComponent, {
      width: '100%',
      panelClass: 'my-dialog-panel',
      backdropClass: 'custom-mat-dialog-bdrop',
      disableClose: true,
      data: {...user}
    });
  }

  OpenDialog() {
    this.dialogRef.open(RegisterComponent, {
      width: '100%',
      panelClass: 'my-dialog-panel',
      backdropClass: 'custom-mat-dialog-bdrop',
      disableClose: true
    });
  }

  canView() {
    return true
  }


  canEdit() {
    return true
  }
}
