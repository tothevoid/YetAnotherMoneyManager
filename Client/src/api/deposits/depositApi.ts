import { DepositMonthSummary } from '../../pages/Deposits/components/DepositStats/depositMonthSummary';
import config from '../../config' 
import { DepositEntity, ServerDepositEntity } from '../../models/deposits/DepositEntity';
import { convertToDateOnly } from '../../shared/utilities/dateUtils';
import { checkPromiseStatus, logPromiseError } from '../../shared/utilities/webApiUtilities';
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

const prepareDepositEntity = (deposit: DepositEntity): ServerDepositEntity => {
    return {
        id: deposit.id,
        initialAmount: deposit.initialAmount,
        name: deposit.name,
        percentage: deposit.percentage,
        accountId: deposit.account?.id,
        estimatedEarn: deposit.estimatedEarn,
        currencyId: deposit.currency?.id,
        // .NET DateOnly cast
        from: convertToDateOnly(deposit.from),
        to: convertToDateOnly(deposit.to),
    }
}

const getQueryDepositsQueryParamers = (monthsFrom: number, monthsTo: number, onlyActive: boolean) => {
    return JSON.stringify({monthsFrom, monthsTo, onlyActive});
}