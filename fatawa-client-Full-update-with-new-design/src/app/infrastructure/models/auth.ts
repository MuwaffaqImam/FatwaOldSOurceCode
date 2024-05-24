import { UserType } from './user';

export interface ILoginRequest {
    email: string;
    password: string;
}

export class LoginRequest implements ILoginRequest {
    email: string = '';
    password: string = '';

    constructor(configOverride?: Partial<ILoginRequest>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}

export interface IOAuthTokenRequest {
    code: string;
    redirect_uri: string;
    state: string;
}

export class OAuthTokenRequest implements IOAuthTokenRequest {
    code: string = '';
    /* tslint:disable:variable-name */
    redirect_uri: string = '';
    state: string = '';

    constructor(configOverride?: Partial<IOAuthTokenRequest>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}

export interface IOAuthRefreshTokenRequest {
    refresh_token: string;
}

export class OAuthRefreshTokenRequest implements IOAuthRefreshTokenRequest {
    refresh_token: string = '';

    constructor(configOverride?: Partial<IOAuthRefreshTokenRequest>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}

export interface IOAuthTokenDTO {
    access_token: string;
    refresh_token: string;
    expires_in: number;
    ext_expires_in: number;
    scope: string;
    token_type: string;
}

export interface IDecodedAuthToken {
    nameid: number;
    unique_name: string;
    role: UserType;
    exp: number;
    preferred_username: string;
}

export class DecodedAuthToken implements IDecodedAuthToken {
    nameid: number;
    unique_name: string = '';
    role: UserType = UserType.None;
    exp: number = 0;
    preferred_username: string = '';

    constructor(configOverride?: Partial<IDecodedAuthToken>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}
