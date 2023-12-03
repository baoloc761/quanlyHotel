import { Component, ViewChild, ElementRef } from '@angular/core';
import { AccountService } from './_services';
import { User } from './_models';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
    selectedLang: string = '';
    user?: User | null;
    routesList: any[] = [
        {
            id: 1,
            path: '/home',
            title: 'Pages.Home.title'
        },
        {
            id: 2,
            path: '/account/users',
            title: 'Pages.UserManagement.title'
        }
    ]

    selectedRoute: number = 1

    constructor(private accountService: AccountService, public translate: TranslateService) {
        this.accountService.user.subscribe(x => this.user = x);
        if (translate.getLangs().length < 1) {
            translate.addLangs(['en', 'vi'])
        }
        let currentLanguage = 'en'
        if (localStorage.getItem('currentLanguage')) {
            currentLanguage = localStorage.getItem('currentLanguage') ?? 'en'
        }
        translate.setDefaultLang(currentLanguage)
        translate.use(currentLanguage)
        this.selectedLang = currentLanguage
    }

    switchLanguage() {
        this.translate.use(this.selectedLang)
        localStorage.setItem('currentLanguage', this.selectedLang)
    }
    
    logout() {
        this.accountService.logout();
    }

    getFlagUrl(lang: string) {
        const prefix = '/assets/images/icons/flags'
        const extension = 'png'
        return `${prefix}/${lang}.${extension}`
    }
}
