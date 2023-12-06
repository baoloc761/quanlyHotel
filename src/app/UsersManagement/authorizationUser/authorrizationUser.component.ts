import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '@app/_services';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'authorizastion-user',
  templateUrl: './authorrizationUser.component.html',
  styleUrls: ['./authorrizationUser.component.scss']
})
export class authorrizationUserComponent implements OnInit {
  userForm!: FormGroup;
  users: any[] = [];
  listPages: any[] = [];
  pageStates: {[pageId: string]: boolean} = {};
  
  constructor(private snackBar: MatSnackBar, 
    private accountService: AccountService, 
    private route: ActivatedRoute,
    private translate: TranslateService) {
      this.userForm = new FormGroup({
        selectedUser: new FormControl()
     });
    }

  ngOnInit() {
    this.users = this.accountService.getAllUsers()
    this.listPages = this.accountService.getListPages()
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

  handleAuthorizationAccount() {
    const formData = this.userForm.value;
    const msg = this.translate.instant('ActionEntityResultAuthorization', 
      { actionName: this.translate.instant('Please'), 
      result:  this.translate.instant(!_.isEmpty(formData.selectedUser) ? 'Success' : 'Error') })
    if (_.isEmpty(formData.selectedUser)) {
      this.openSnackBar(msg, this.translate.instant('Empty'))
      return
    }

    console.log(this.listPages);
  }
}
