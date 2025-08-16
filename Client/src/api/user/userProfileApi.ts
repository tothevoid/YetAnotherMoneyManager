import config from "../../config";
import { UserProfileEntity } from "../../models/user/UserProfileEntity";
import { checkPromiseStatus, logPromiseError } from "../../shared/utilities/webApiUtilities";
import { updateEntity } from "../basicApi";
import { prepareUserProfileRequest } from "./userProfileApiMapping";

const basicUrl = `${config.api.URL}/UserProfile`;

export const getUserProfile = async (): Promise<UserProfileEntity> =>  {
    return await fetch(basicUrl, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
}

export const updateUserProfile = async (userProfile: UserProfileEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareUserProfileRequest(userProfile));
}