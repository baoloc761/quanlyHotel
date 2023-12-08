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
  pageIdRole: { id: string }[] = [];
  pageStates: {[pageId: string]: boolean} = {};
  
  constructor(private snackBar: MatSnackBar, 
    private accountService: AccountService, 
    private route: ActivatedRoute,
    private translate: TranslateService) {}

  ngOnInit() {
    this.users = this.accountService.getAllUsers();
    this.listPages = this.accountService.getListPages();
    this.listPages.forEach(page => {
      this.pageStates[page.id] = false
    })
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

  handleCheckboxChange(pageId: any) {
    const isChecked = this.pageStates[pageId];
    const formData = this.selectedUserId;
    if (_.isEmpty(formData)) return;

    if (isChecked) {
        const existingItem = this.pageIdRole.find(item => item.id === pageId);
        if (!existingItem) {
            this.pageIdRole.push({ id: pageId });
        }
    } else {
        const index = this.pageIdRole.findIndex(item => item.id === pageId);
        if (index !== -1) {
            this.pageIdRole.splice(index, 1);
        }
    }
    return this.pageIdRole || [];
  }

  handleAuthorizationAccount() {
    const UserId = this.selectedUserId;
    const findpageIdRole = _.filter(this.pageIdRole, x => typeof x === 'object' && 'id' in x);
    const msg = this.translate.instant('ActionEntityResultAuthorization', {
      actionName: this.translate.instant('PleaseChooseOptionPage'),
      result: this.translate.instant(!_.isEmpty(UserId) ? 'Success' : 'Error')
    })
  
    if (_.isEmpty(UserId)) {
      this.openSnackBar(msg, this.translate.instant('Empty'));
      return;
    }
    _.forEach(this.listPages, (page) => {
      const isPageSelected = findpageIdRole.some(role => role.id === page.id)
      if (isPageSelected) {
        if (!page.roleUser.includes(UserId)) {
          page.roleUser.push(UserId)
        }
      } else {
        const index = page.roleUser.indexOf(UserId)
        if (index !== -1) {
          page.roleUser.splice(index, 1)
        }
      }
    })
    console.log('this.listPages', this.listPages)
  }
}