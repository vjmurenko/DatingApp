import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent implements OnInit {

  public errorMessage;


  constructor(private router: Router) {
    let navigation = router.getCurrentNavigation();
    this. errorMessage = navigation.extras.state.error;
  }

  ngOnInit(): void {
  }

}
