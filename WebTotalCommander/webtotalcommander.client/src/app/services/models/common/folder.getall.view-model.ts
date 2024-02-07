import { FolderFileViewModel } from "./folder.file.view-model";
import { PaginationMetaDataView } from "./pagination.data";

export class FolderGetAllViewModel{
    folderFile:Array<FolderFileViewModel>=[];
    paginationMetaData:PaginationMetaDataView=new PaginationMetaDataView();
}