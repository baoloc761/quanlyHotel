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
  selectedUserId: string = '';
  users: any[] = [];
  listPages: any[] = [];
  pageStates: {[pageId: string]: boolean} = {};
  
  constructor(private snackBar: MatSnackBar, 
    private accountService: AccountService, 
    private route: ActivatedRoute,
    private translate: TranslateService) {}

  ngOnInit() {
    this.users = this.accountService.getAllUsers();
    this.listPages = this.accountService.getListPages();
    this.listPages.forEach(page => {
      this.pageStates[page.id] = false;
    });
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

  handleCheckboxChange() {
    const formData = this.selectedUserId;
    const isCheckedPage = _.some(this.pageStates, value => value === true)
    if(_.isEmpty(formData)) return
    return isCheckedPage || undefined
  }

  handleAuthorizationAccount() {
    const formData = this.selectedUserId;
    const isCheckedPageUserRole = this.handleCheckboxChange()
    const msg = this.translate.instant('ActionEntityResultAuthorization', 
      { actionName: !_.isNil(isCheckedPageUserRole) ? this.translate.instant('Please') : this.translate.instant('PleaseChooseOptionPage'), 
      result:  this.translate.instant(!_.isEmpty(formData) ? 'Success' : 'Error') })
    if (_.isEmpty(formData)) {
      this.openSnackBar(msg, this.translate.instant('Empty'))
      return
    }
    if (!isCheckedPageUserRole) {
      this.openSnackBar(msg, this.translate.instant('Empty'))
      return
    }
    console.log(this.listPages);
  }
}
