import config from "../../config";
import { UserProfileEntity } from "../../models/user/UserProfileEntity";
import { checkPromiseStatus, logPromiseError } from "../../utils/PromiseUtils";
import { updateEntity } from "../basicApi";

const basicUrl = `${config.api.URL}/UserProfile`;

export const getUserProfile = async (): Promise<UserProfileEntity> =>  {
    return await fetch(basicUrl, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);    
}

export const updateUserProfile = async (userProfile: UserProfileEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, userProfile);
}