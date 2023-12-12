import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { AccountService } from './_services';
import { User } from './_models';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    selectedLang: string = '';
    user?: User | null;
    routesList: any[] = [];
    selectedRoute: number = 1;
    private subscription: Subscription | undefined

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

    ngOnInit() {
        this.subscription = this.accountService.getMenusList().subscribe(
            (data) => {
                this.routesList = data;
            },
            (error) => {
                console.error('Error fetching data:', error);
                this.routesList = [];
            }
        );
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
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
