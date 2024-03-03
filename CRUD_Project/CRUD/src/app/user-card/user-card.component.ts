import { Component, Input, OnInit  } from '@angular/core';

import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { UserModalComponent } from '../modals/user-modal/user-modal.component';
import { DeleteUserModalComponent } from '../modals/delete-user-modal/delete-user-modal.component';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() user!: User;
  hobbies: string[] = [];
  skillsets: string[] = [];

  constructor(private userService: UserService, private dialog: MatDialog) { }
  
  ngOnInit(): void {
    this.userService.getAvailableHobbies().subscribe(hobbies => {
      this.hobbies = hobbies.filter(hobby => this.user.userHobbies.includes(hobby.id)).map(hobby => hobby.title);
    });

    this.userService.getAvailableSkillsets().subscribe(skillsets => {
      this.skillsets = skillsets.filter(skillset => this.user.userSkillsets.includes(skillset.id)).map(skillset => skillset.title);
    });
  }

  openDialog(userId: any){
    this.dialog.open(UserModalComponent, {
      width: '60%',
      enterAnimationDuration: '300ms',
      exitAnimationDuration: '300ms',
      data: {
        data: this.user,
        userId: userId
      }
    });
  }

  openConfirmDialog(userId: any, userName: any){
    this.dialog.open(DeleteUserModalComponent, {
      width: '40%',
      enterAnimationDuration: '300ms',
      exitAnimationDuration: '300ms',
      data: {
        data: this.user,
        userId: userId,
        userName: userName
      }
    })
  }
}
