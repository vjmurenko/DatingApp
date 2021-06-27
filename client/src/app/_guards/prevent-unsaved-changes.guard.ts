import {Injectable} from '@angular/core';
import {CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree} from '@angular/router';
import {Observable} from 'rxjs';
import {EditMemberComponent} from '../members/edit-member/edit-member.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(
    component: EditMemberComponent): boolean {
    if(component.editForm.dirty){
      return confirm("Are you sure you want to continue? Any unsaved changes will be lost")
    }
    return true;
  }

}
