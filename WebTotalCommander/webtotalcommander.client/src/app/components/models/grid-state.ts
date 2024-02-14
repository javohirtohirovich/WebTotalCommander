import { CompositeFilterDescriptor, SortDescriptor } from "@progress/kendo-data-query";
export interface GridState {
    filter: CompositeFilterDescriptor,
    sort: SortDescriptor[] | null;
}