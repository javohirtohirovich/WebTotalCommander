import { Injectable, inject } from "@angular/core";
import { FileApiService } from "../api/file.api-service";
import { FileViewCreateModel } from "./models/file/file.view-create.model";
import { Observable } from "rxjs";
import { HttpEvent } from "@angular/common/http";

@Injectable({ providedIn: "root" })
export class FileService {
    //Inject FolderApiService
    private fileApiService: FileApiService = inject(FileApiService)

    public addFile(file:FileViewCreateModel):Observable<any>{
        return this.fileApiService.addFile(file);
    }
    public downloadFile(filePath:string):Observable<any>{
        return this.fileApiService.downloadFile(filePath);
    }
}