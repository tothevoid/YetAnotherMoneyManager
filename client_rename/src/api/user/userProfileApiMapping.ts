import { UserProfileEntity, UserProfileEntityRequest } from "../../models/user/UserProfileEntity";

export const prepareUserProfileRequest = (userProfile: UserProfileEntity): UserProfileEntityRequest => {
    return {
        id: userProfile.id,
        currencyId: userProfile.currency.id,
        languageCode: userProfile.languageCode
    }
}