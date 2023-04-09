import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  formModel = this._fb.group({
    UserName: ['', Validators.required],
    Password: ['', Validators.required]
  })

  constructor(private _fb:FormBuilder, private service:UserService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    if(localStorage.getItem('token') != null) {
      this.router.navigateByUrl('/home');
    }
  }

  onSubmit() {
    var body = {
      UserName: this.formModel.value.UserName,
      Password: this.formModel.value.Password
    };

    this.service.login(body).subscribe(
      (res:any) => {
        localStorage.setItem('token', res.token);
        this.router.navigateByUrl('/home');
      },
      error => {
        if(error.status == 400) {
          this.toastr.error('Incorrect username or password', 'Authentication failed')
        }
        else {
          console.log(error);
        }
      }
    );
  }

}
