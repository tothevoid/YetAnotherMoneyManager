import config from "../../config";
import { GlobalDashboard } from "../../models/dashboard/DashboardEntity";
import { checkPromiseStatus, logPromiseError } from "../../shared/utilities/webApiUtilities";

const basicUrl = `${config.api.URL}/Dashboard`;

export const getDashboard = async (): Promise<GlobalDashboard | void> =>  {
    return await fetch(`${basicUrl}`, {method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
}