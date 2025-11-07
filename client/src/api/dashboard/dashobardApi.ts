import config from "../../config";
import { GlobalDashboard } from "../../models/dashboard/DashboardEntity";
import { getEntity } from "../basicApi";

const basicUrl = `${config.api.URL}/Dashboard`;

export const getDashboard = async (): Promise<GlobalDashboard | void> =>  {
    return await getEntity<GlobalDashboard>(basicUrl);
}