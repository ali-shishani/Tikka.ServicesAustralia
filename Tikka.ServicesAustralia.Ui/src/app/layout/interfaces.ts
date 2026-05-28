/*
Interface for the device
*/
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
  Id: string;
  username: string;
  email: string;
  dateOfBirth?: Date;
  gender: string;
  isEmailConfirmed: boolean;
}

export interface getUsersResponseWrapper {
  data: getUsersResponse[];
}
