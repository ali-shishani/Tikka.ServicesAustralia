
export interface ApiException {
  message: string;
  details: string;
}

export interface DeviceInformationResponse {
  deviceName: string;
  clientId: string;
  productId: string;
  organisationRA: string;
  serviceNapsId: string;
  agedCareResidentialServiceId: string;
  agedCareHomeServiceId: string;
  tokenAud: string;
  deviceExpiry: string;
  keyExpiry: string;
}

export interface DeviceInformationResponseWrapper {
  data: DeviceInformationResponse;
}

export interface ActivateDeviceResponseWrapper {
  data: string;
}

export interface RefreshDeviceKeyResponseWrapper {
  data: string;
}

export interface getUsersResponse {
  id: string;
  userName: string;
  email: string;
  dateOfBirth: Date;
  gender: string;
  isEmailConfirmed: boolean;
}

export interface getUsersResponseWrapper {
  data: getUsersResponse[];
  errors: ApiException[];
}

export interface UpdateUserRequest {
  userId: string;
  username: string;
  dateOfBirth: Date;
  gender: number;
  isEmailConfirmed: boolean;
}

export interface deleteUsersResponseWrapper {
  data: Boolean;
  errors: ApiException[];
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export interface changePasswordResponseWrapper {
  data: Boolean;
  errors: ApiException[];
}
