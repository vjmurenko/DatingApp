import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AccountService} from '../_services/account.service';
import {AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators} from '@angular/forms';
import {validate, ValidateFn} from 'codelyzer/walkerFactory/walkerFn';
import {Router} from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();

  registerForm: FormGroup;
  maxDate: Date;
  validateErrors: string[] = [];

  constructor(private accountService: AccountService, private fb: FormBuilder, private router: Router) {
  }

  ngOnInit() {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      gender: ['male'],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
  }

  matchValues(nameControl: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[nameControl]?.value ? null : {isMatching: true};
    };
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(response => {
        this.router.navigateByUrl('/members');
      },
      error => {
        this.validateErrors = error;
      });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
