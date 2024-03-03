import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-delete-user-modal',
  templateUrl: './delete-user-modal.component.html',
  styleUrls: ['./delete-user-modal.component.css']
})
export class DeleteUserModalComponent implements OnInit{

  id = new FormControl();
  deleteData!: User;
  validationErrors: string[] | undefined;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<DeleteUserModalComponent>,
    private userService: UserService,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
     if (this.data) {
      this.id.setValue(this.data.userId);
    }
  }

  Delete(deleteUserId: number){
    this.userService.deleteUser(deleteUserId).subscribe({
      next: (response: any) => {
        this.dialogRef.close();
        this.userService.notifyUsersUpdated();
        this.toastr.success(response.message); // Display success message from response
      },
      error: (error: any) => {
        if (error && error.error && error.error.message) {
          this.toastr.error(error.error.message);
        } else {
          this.toastr.error('An error occurred while deleting the user.'); // Default error message
        }
      }
    });
  }
}
