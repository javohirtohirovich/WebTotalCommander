import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { GridModule } from '@progress/kendo-angular-grid';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FooterComponent } from './components/footer/footer.component';
import { HomeComponent } from './components/home/home.component';
import { HeaderComponent } from './components/header/header.component';
import { LayoutComponent } from './components/layout/layout.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

////////////
import { ToastrModule} from 'ngx-toastr';
import { NavigationModule } from "@progress/kendo-angular-navigation";
import { ButtonsModule } from "@progress/kendo-angular-buttons";
import { DialogsModule } from "@progress/kendo-angular-dialog";

import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';
import { SVGIconModule } from '@progress/kendo-angular-icons';
import { InputsModule } from "@progress/kendo-angular-inputs";
import { PagerModule } from "@progress/kendo-angular-pager";
import { LabelModule } from '@progress/kendo-angular-label';

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HomeComponent,
    HeaderComponent,
    LayoutComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,GridModule, FormsModule,CommonModule, NavigationModule,
    ToastrModule.forRoot({
      timeOut: 2000,
    positionClass: 'toast-top-right',
    preventDuplicates: true,
    }),BrowserAnimationsModule,ButtonsModule,SVGIconModule,DialogsModule,InputsModule,FormsModule,PagerModule,LabelModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
