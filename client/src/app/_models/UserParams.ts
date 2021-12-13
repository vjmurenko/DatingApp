import {User} from './User';
import {PageParams} from './PageParams';

export class UserParams extends PageParams {

  maxAge: number = 99;
  minAge: number = 18;
  gender: string;
  orderBy: string = "lastActive"

  constructor(user: User) {
    super();
    this.gender = user.gender === 'male' ? 'female' : 'male';
  }
}