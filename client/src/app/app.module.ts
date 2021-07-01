import {BrowserModule} from '@angular/platform-browser';
import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NavComponent} from './nav/nav.component';
import {HomeComponent} from './home/home.component';
import {RegisterComponent} from './register/register.component';
import {ListsComponent} from './lists/lists.component';
import {MemberDetailComponent} from './members/member-detail/member-detail.component';
import {MemberListComponent} from './members/member-list/member-list.component';
import {MessagesComponent} from './messages/messages.component';
import {FormsModule} from '@angular/forms';
import {SharedModule} from './_modules/shared.module';
import {TestErrorComponent} from './_errors/test-error/test-error.component';
import {NotFoundComponent} from './_errors/not-found/not-found.component';
import {ServerErrorComponent} from './_errors/server-error/server-error.component';
import {ErrorInterceptor} from './_interceptors/error.interceptor';
import { MemberCardComponent } from './members/member-card/member-card.component';
import {JwtInterceptor} from './_interceptors/jwt.interceptor';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import {LoadingInterceptor} from './_interceptors/loading.interceptor';
import {NgxSpinnerModule} from 'ngx-spinner';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    ListsComponent,
    MemberDetailComponent,
    MemberListComponent,
    MessagesComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    SharedModule,
    NgxSpinnerModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [AppComponent]
})
export class AppModule {
}
