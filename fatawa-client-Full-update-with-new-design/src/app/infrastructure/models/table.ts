export interface ITableColumns {
    columnIndex: number;
    columnId: string;
    columnName: string;
}

export class TableColumns implements ITableColumns {
    columnIndex: number = 0;
    columnId: string = '';
    columnName: string = '';

    constructor(configOverride?: Partial<ITableColumns>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}
