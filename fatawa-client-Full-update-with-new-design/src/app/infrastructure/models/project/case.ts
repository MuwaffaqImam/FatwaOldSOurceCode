export type decisionType = 'hold' | 'release' | '';
export type statusType = 'pending' | 'completed' | '';

export interface IQueryStatus {
    status: statusType;
}

export class QueryStatus implements IQueryStatus {
    status: statusType;

    constructor(configOverride?: Partial<IQueryStatus>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}

export interface ICase {
    id: number;
    lastName: string;
    firstName: string;
    middleName: string;
    dob: string;
    gender: string; // What choices are allowed? Convert to enum
    status: statusType;
    decision: decisionType;
}

export interface ICaseUpdated {
    id: number;
    name: string;
    dob: string;
    gender: string;
    status: statusType;
    decision: decisionType;
}

export class CaseUpdated implements ICaseUpdated {
    id: number = 0;
    name: string = '';
    dob: string = '';
    gender: string = '';
    status: statusType = 'pending';
    decision: decisionType = '';

    constructor(configOverride?: Partial<ICaseUpdated>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}

export interface ICaseResponse {
    results: ICase[];
}

export interface ICaseRequest {
    id: number;
    decision: decisionType;
}

export class CaseRequest implements ICaseRequest {
    id: number = 0;
    decision: decisionType = 'hold';

    constructor(configOverride?: Partial<ICaseRequest>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}

export interface ICaseDTO {
    results: ICaseUpdated[];
}

export class CaseDTO implements ICaseDTO {
    results: ICaseUpdated[] = [];

    constructor(configOverride?: Partial<ICaseDTO>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}
