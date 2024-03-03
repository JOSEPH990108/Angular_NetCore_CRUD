import { Component } from '@angular/core';
import { UserService } from './_services/user.service';
import { Hobby } from './_models/hobby';
import { User } from './_models/user';
import { Skillset } from './_models/skillset';
import { MatDialog } from '@angular/material/dialog';
import { UserModalComponent } from './modals/user-modal/user-modal.component';
import { Pagination } from './_models/pagination';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'CRUD';
  users: User[] = [];
  hobbies: Hobby[] = [];
  skillsets: Skillset[] = [];
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 6;

  constructor(
    private userService: UserService, private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadUsers(); 
    
    // Subscribe to user creation events
    this.userService.onUserUpdated().subscribe(() => {
      this.loadUsers(); // Refresh user list when a new user is created
    });

  }

  loadUsers(): void {
    this.userService.getAllFreelancer(this.pageNumber, this.pageSize).subscribe({
      next: response => {
        if(response.result && response.pagination){
          this.users = response.result;
          this.pagination = response.pagination;
        }
      },
      error: error => console.log(error),
      complete: () => console.log('Request has completed'),
    });
  }

  pageChanged(event: any){
    if(this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.loadUsers();
    }
  }

  openDialog(){
    this.dialog.open(UserModalComponent, {
      width: '60%',
      enterAnimationDuration: '300ms',
      exitAnimationDuration: '300ms',
    })
  }
}
