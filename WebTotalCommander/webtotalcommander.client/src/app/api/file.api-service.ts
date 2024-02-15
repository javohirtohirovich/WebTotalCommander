import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { FileCreateModel } from './models/file/file.create-model';
import { Observable, catchError } from 'rxjs';
import { FileDeleteModel } from './models/file/file.delete-model';
import { FileEditModel } from './models/file/file.edit-model';

@Injectable({ providedIn: 'root' })
export class FileApiService {
    //Variable HttpClient Inject
    private client: HttpClient = inject(HttpClient);

    //Variable Backend URL
    private url: string = 'https://localhost:7251/api/file';

    //Function (request) Upload File
    public addFile(file: FileCreateModel): Observable<any> {
        const formData: FormData = new FormData();
        formData.append('FilePath', file.filePath);
        formData.append('File', file.file!);
        return this.client.post(this.url, formData);
    }

    //Function (request) Download File
    public downloadFile(filePath: string): Observable<any> {
        return this.client.get(`${this.url}?filePath=${filePath}`, {
            responseType: 'blob',
        });
    }

    //Function (request) Delete File
    public deleteFile(fileDeleteModel: FileDeleteModel): Observable<any> {
        return this.client.delete(this.url, { body: fileDeleteModel });
    }

    //Function (request) Get Txt File (for edit)
    public getTxtFile(filePath: string): Observable<any> {
        return this.client
            .get(`${this.url}/Text?filePath=${filePath}`, {
                responseType: 'blob',
            }).pipe(catchError((error) => {
                    throw error;
                })
            );
    }

    //Function (request) Txt File
    public editTxtFile(fileEditModel: FileEditModel): Observable<any> {
        const formData: FormData = new FormData();
        formData.append('file', fileEditModel.file!);
        return this.client.put(
            `${this.url}/Text?filePath=${fileEditModel.filePath}`,
            formData
        );
    }
}
