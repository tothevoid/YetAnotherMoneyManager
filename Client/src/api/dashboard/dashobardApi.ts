import config from "../../config";
import { Dashboard } from "../../models/dashboard/DashboardEntity";
import { checkPromiseStatus, logPromiseError } from "../../utils/PromiseUtils";

const basicUrl = `${config.api.URL}/Dashboard`;

export const getDashboard = async (): Promise<Dashboard | void> =>  {
    return await fetch(`${basicUrl}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
}