import { NgModule } from "@angular/core";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatDialogModule } from "@angular/material/dialog";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from '@angular/material/icon';

@NgModule({
    exports: [
        MatInputModule,
        MatSelectModule,
        MatAutocompleteModule,
        MatDialogModule,
        MatButtonModule,
        MatIconModule
    ]
})
export class MaterialModule{}