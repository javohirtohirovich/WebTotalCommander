import { FileGetModel } from "./file.get.model";
import { FolderGetModel } from "./folder.get.model";


export interface FolderGetAllModel{
    folders:Array<FolderGetModel>;
    files:Array<FileGetModel>;
}