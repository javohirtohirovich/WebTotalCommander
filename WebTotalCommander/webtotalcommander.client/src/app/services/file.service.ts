import { Injectable, inject } from "@angular/core";
import { FileApiService } from "../api/file.api-service";
import { FileViewCreateModel } from "./models/file/file.view-create.model";
import { Observable } from "rxjs";
import { FileViewDeleteModel } from "./models/file/file.view-delete.model";
import { FileViewEditModel } from "./models/file/file.view-edit.model";

@Injectable({ providedIn: "root" })
export class FileService {
    //Variable Inject FolderApiService
    private fileApiService: FileApiService = inject(FileApiService)

    //Function (request) Upload File
    public addFile(file:FileViewCreateModel):Observable<any>{
        return this.fileApiService.addFile(file);
    }

    //Function (request) Download File
    public downloadFile(filePath:string):Observable<any>{
        return this.fileApiService.downloadFile(filePath);
    }

    //Function (request) Delete File
    public deleteFile(fileDeleteModel:FileViewDeleteModel):Observable<any>{
        return this.fileApiService.deleteFile(fileDeleteModel);
    }

    //Function (request) Get Txt File
    public getTxtFile(filePath:string):Observable<any>{
        return this.fileApiService.getTxtFile(filePath);
    }
    
    //Function (request) Edit Txt File
    public editTxtFile(fileEditModel:FileViewEditModel):Observable<any>{
        return this.fileApiService.editTxtFile(fileEditModel);
    }
}