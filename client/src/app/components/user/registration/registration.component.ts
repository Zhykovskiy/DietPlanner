import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  public formModel = this._fb.group({
    userName: ['', Validators.required],
    email: ['', Validators.email],
    passwords: this._fb.group({
      password: ['', [Validators.required, Validators.minLength(4)]],
      confirmPassword: ['', Validators.required]
    },{validators: this.comparePasswords}),
    firstName: ['', Validators.required],
    lastName: ['', Validators.required]
  });

  constructor(private _fb:FormBuilder, public service: UserService, private toastr:ToastrService, private router:Router) { }

  ngOnInit(): void {
    this.formModel.reset();
  }

  comparePasswords(_fb:FormGroup) {
    let confirmPasswordCtrl = _fb.get('ConfirmPassword');

    if(confirmPasswordCtrl?.errors == null || 'passwordMismatch' in confirmPasswordCtrl.errors) {
      if(_fb.get('Password')?.value != confirmPasswordCtrl?.value) {
        confirmPasswordCtrl?.setErrors({passwordMismatch:true});
      } 
      else {
        confirmPasswordCtrl?.setErrors(null);
  }
    }
  }

  onSubmit() {
    var body = {
      userName: this.formModel.value.userName,
      email: this.formModel.value.email,
      password: this.formModel.value.passwords.password,
      firstName: this.formModel.value.firstName,
      lastName: this.formModel.value.lastName
    };

    this.service.register(body).subscribe(
      (res:any) => {
        if(res.succeeded){
          this.router.navigate(['user/login']);;
          this.toastr.success('User created', 'Registration successful');
        }
        else {
          res.errors.forEach((element:any) => {
            switch(element.code){
              case "DuplicateUserName":
                this.toastr.error('UserName is already taken', 'Registration failed')
                break;

              default:
                this.toastr.error(element.description, 'Registration failed')
                break;
            }
          });
        }
      },
      error => {
        console.log(error);
      }
    );
  }
}
