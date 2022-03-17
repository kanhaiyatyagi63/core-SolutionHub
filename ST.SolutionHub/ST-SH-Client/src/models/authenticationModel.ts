export class AuthenticateResponseModel {
    public id: string;
    public name: string;
    public roles: string[];
    public token: string;
    public refreshToken: string;
    public profileImgUrl: string;
}
export class AuthenticateRequestModel {
    public userName: string;
    public password: string;
}

export declare class SocialUser {
    provider: string;
    id: string;
    email: string;
    name: string;
    photoUrl: string;
    firstName: string;
    lastName: string;
    authToken: string;
    idToken: string;
    authorizationCode: string;
    response: any;
}

