import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Hobby } from 'src/app/_models/hobby';
import { Skillset } from 'src/app/_models/skillset';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-user-modal',
  templateUrl: './user-modal.component.html',
  styleUrls: ['./user-modal.component.css']
})
export class UserModalComponent implements OnInit {

  modalForm: FormGroup = new FormGroup({});
  username = new FormControl();
  mail = new FormControl();
  phoneNumber = new FormControl();
  userHobbies = new FormControl();
  userSkillsets = new FormControl();
  validationErrors: string[] | undefined;
  hobbyList: Hobby[] = [];
  skillsetList: Skillset[] = [];
  editData!: User;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<UserModalComponent>,
    private userService: UserService,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.userService.getAvailableHobbies().subscribe((hobbies: Hobby[]) => {
      this.hobbyList = hobbies;
    });
    this.userService.getAvailableSkillsets().subscribe((skillsets: Skillset[]) => {
      this.skillsetList = skillsets;
    });

    if (this.data) {
      console.log(this.data.userId);
      this.setModalData(this.data.userId);
    }
  }

  initializeForm(){
    this.modalForm = this.fb.group({
      username: ['', Validators.required],
      mail: ['', [Validators.required, Validators.email]], // Using Validators.email for email validation
      phoneNumber: ['', Validators.required],
      userHobbies: ['', Validators.required],
      userSkillsets: ['', Validators.required],
    });
  }

  setModalData(userId: number){
    this.userService.getFreelancerById(userId).subscribe(item =>{
      this.editData = item;
      this.modalForm.patchValue({
        username: this.editData.username,
        mail: this.editData.mail,
        phoneNumber: this.editData.phoneNumber,
        userHobbies: this.editData.userHobbies,
        userSkillsets: this.editData.userSkillsets
    });
  });
}

  submitForm(userId: number | undefined) {
    console.log(userId);
    console.log(this.modalForm.value);
    if (this.modalForm.valid) {
      const userData: User = this.modalForm.value;

      if (userId) {
        this.updateUser(userId, userData);
      } else {
        this.createUser(userData);
      }
    } else {
      Object.keys(this.modalForm.controls).forEach(controlName => {
        const control = this.modalForm.get(controlName);

        // Check if the control is invalid
        if (control && control.invalid) {
            // Display Toastr message for each invalid control
            if (controlName === 'mail' && control.hasError('email') && !this.isEmailFormat(control.value)) {
                this.toastr.error('Invalid email format. Please enter a valid email address.');
            } else {
                this.toastr.error(`Please fill in ${controlName}.`);
            }
        }
      });
    }
  }

  createUser(userData: User) {
    this.userService.createUser(userData).subscribe({
      next: () => {
        this.dialogRef.close();
        this.userService.notifyUsersUpdated();
        this.toastr.success('User created successfully!');
      },
      error: error => {
        if (error && error.error && error.error.message) {
          this.toastr.error(error.error.message);
        } else {
          this.toastr.error('An error occurred while creating the user.');
        }
      }
    });
  }

  updateUser(userId: number, userData: User) {
    this.userService.updateUser(userId, userData).subscribe({
      next: () => {
        this.dialogRef.close();
        this.userService.notifyUsersUpdated();
        this.toastr.success('User updated successfully!');
      },
      error: error => {
        if (error && error.error && error.error.message) {
          this.toastr.error(error.error.message);
        } else {
          this.toastr.error('An error occurred while updating the user.');
        }
      }
    });
  }

  //Validate Email Input
  isEmailFormat(email: string): boolean {
    // Regular expression to validate email format
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(email);
  }

}

