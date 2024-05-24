import { State } from '../shared/Services/CommonMemmber';

export interface DynamicColumn {
    headerName: string;
    icon: string;
    status?: State;
    childColumn?: SubChildrenColumn[];
}
export interface SubChildrenColumn {
    name: string;
    status: State;
}
