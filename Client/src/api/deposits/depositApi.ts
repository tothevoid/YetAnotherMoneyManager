import { DepositMonthSummary } from '../../components/deposits/DepositStats/depositMonthSummary';
import config from '../../config' 
import { DepositEntity } from '../../models/deposits/DepositEntity';
import { convertToDateOnly } from '../../utils/DateUtils';
import { checkPromiseStatus, logPromiseError } from '../../utils/PromiseUtils';
import { DepositsRange } from '../../models/deposits/depositsRange';
import { createEntity, deleteEntity, updateEntity } from '../basicApi';

const basicUrl = `${config.api.URL}/Deposit`;

export const getDeposits = async (monthsFrom: number, monthsTo: number, onlyActive: boolean): Promise<DepositEntity[]> => {
    const url = `${basicUrl}/GetAll`;
    const deposits = await fetch(url, {method: "POST", 
        body: getQueryDepositsQueryParamers(monthsFrom, monthsTo, onlyActive),
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .then((deposits: DepositEntity[]) => 
            deposits.map((deposit: DepositEntity) => {
                const from = new Date(deposit.from);
                const to = new Date(deposit.to);
                return {...deposit, from, to} as DepositEntity;
            })
        )
        .catch(logPromiseError);
  
    return deposits ?
    deposits: 
        [] as DepositEntity[];
};

export const createDeposit = async (createdDeposit: DepositEntity): Promise<DepositEntity | void> => {
    return await createEntity(basicUrl, prepareDepositEntity(createdDeposit));
}

export const updateDeposit = async (modifiedDeposit: DepositEntity): Promise<boolean> => {
    return await updateEntity(basicUrl, prepareDepositEntity(modifiedDeposit));
}

export const deleteDeposit = async (depositId: string): Promise<boolean> => {
    return await deleteEntity(basicUrl, depositId)
}

export const getDepositsRange = async (): Promise<DepositsRange | null> => {
    const url = `${basicUrl}/GetDepositsRange`;
    const result:  DepositsRange | null = await fetch(url, { method: "GET"})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);

    return result;
}

export const getDepositsSummary = async (monthsFrom: number, monthsTo: number, onlyActive: boolean): Promise<DepositMonthSummary | null> => {
    const url = `${basicUrl}/GetDepositsSummary`;
    const summary: DepositMonthSummary | null = await fetch(url, {method: "POST",  
        body: getQueryDepositsQueryParamers(monthsFrom, monthsTo, onlyActive),
        headers: {"Content-Type": "application/json"}})
        .then(checkPromiseStatus)
        .then((response: Response) => response.json())
        .catch(logPromiseError);
  
    return summary;
};

const prepareDepositEntity = (deposit: DepositEntity): DepositEntity => {
    const serverDeposit = {...deposit};

    // .NET DateOnly cast
    serverDeposit.from = convertToDateOnly(deposit.from);
    serverDeposit.to = convertToDateOnly(deposit.to);

    serverDeposit.currencyId = deposit.currency.id;

    return serverDeposit;
}

const getQueryDepositsQueryParamers = (monthsFrom: number, monthsTo: number, onlyActive: boolean) => {
    return JSON.stringify({monthsFrom, monthsTo, onlyActive});
}