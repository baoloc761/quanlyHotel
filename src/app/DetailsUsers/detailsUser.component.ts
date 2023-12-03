import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '@app/_services';
@Component({ templateUrl: './detailsUser.component.html', styleUrls: [ './detailsUser.component.scss'] })
export class DetailsUsersComponent implements OnInit {
  user: any = {}

  constructor(private route: ActivatedRoute, private accountService: AccountService) {}

  ngOnInit(): void {
    this.user = this.accountService.getUserById(this.id)
  }

  get id(): string {
    const id = this.route.snapshot.paramMap.get('id') ?? ''
    return id
  }
}