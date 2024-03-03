import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';

import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { MaterialModule } from './Matetial.Module';
import { PaginationModule } from 'ngx-bootstrap/pagination';

// Components
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { UserCardComponent } from './user-card/user-card.component';
import { UserModalComponent } from './modals/user-modal/user-modal.component';
import { DeleteUserModalComponent } from './modals/delete-user-modal/delete-user-modal.component';
// Interceptors
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { ErrorInterceptor } from './_interceptors/error.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    UserCardComponent,
    UserModalComponent,
    DeleteUserModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    MaterialModule,
    ReactiveFormsModule,
    ToastrModule.forRoot({
      positionClass: "toast-top-right",
      newestOnTop: true,
      tapToDismiss: true,
      progressBar: true
    }),
    NgxSpinnerModule.forRoot({
      type: 'ball-atom'
    }),
    PaginationModule.forRoot()
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
