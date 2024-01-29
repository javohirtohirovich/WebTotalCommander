import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { FileCreateModel } from "./models/file/file.create-model";
import { Observable } from "rxjs";

@Injectable({providedIn:"root"})
export class FileApiService{
    private client: HttpClient=inject(HttpClient);
    private url:string="https://localhost:7251/api/file";
    
    //Add File
    public addFile(file:FileCreateModel):Observable<any>{
        const formData: FormData = new FormData();
        formData.append("FilePath",file.filePath);
        formData.append("File",file.file!);
        return this.client.post(this.url,formData)
    }
}
