﻿import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NullComponent } from './../../app/null.component';
import { SignInComponent } from './../components/signin.component';
import { SignupComponent } from './../components/signup.component';
import { MainPageComponent } from './../components/main-page.component';
import { MainComponent } from './../components/main.component';
import { AccountInfoComponent } from './../components/account-info.component';
import { CreateFarmComponent } from './../components/farm-create.component';
import { CreateFarmCmpComponent } from './../components/farm-component-create.component';
import { ReportComponent } from './../components/report.component';

const routes: Routes = [
    { path: '', component: NullComponent },
    { path: 'signin', component: SignInComponent },
    { path: 'signup', component: SignupComponent },
    {
        path: 'farmiot', component: MainComponent, children:[
            { path: "", redirectTo: "main", pathMatch: "prefix" },
            { path: "main", component: MainPageComponent },
            { path: "profile", component: AccountInfoComponent },
            { path: "newfarm", component: CreateFarmComponent },
            { path: "farm/:id/newfarmcmp", component: CreateFarmCmpComponent },
            { path: "farm/component/:id", component: ReportComponent }
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MainRoutesModule { }