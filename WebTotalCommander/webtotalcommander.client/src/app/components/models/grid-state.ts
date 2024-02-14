import { CompositeFilterDescriptor } from "@progress/kendo-data-query";
export interface GridState {
    filter: CompositeFilterDescriptor,
    sort: { dir?: string, field: string } | null;
}