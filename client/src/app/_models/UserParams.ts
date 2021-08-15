import {User} from './User';

export class UserParams {
  pageNumber: number = 1;
  pageSize: number = 5;
  maxAge: number = 99;
  minAge: number = 18;
  gender: string;
  orderBy: string = "lastActive"

  constructor(user: User) {
    this.gender = user.gender === 'male' ? 'female' : 'male';
  }
}