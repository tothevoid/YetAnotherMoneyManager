import { CurrencyEntity } from "../currencies/CurrencyEntity";
import { BrokerAccountTypeEntity } from "./BrokerAccountTypeEntity";
import { BrokerEntity } from "./BrokerEntity";

interface CommonBrokerAccountEntity {
	id: string,
	name: string,
	initialValue: number,
	currentValue: number,
	mainCurrencyAmount: number,
}

export interface BrokerAccountEntityRequest extends CommonBrokerAccountEntity{
	typeId: string,
	currencyId: string, 
	brokerId: string,
}

export interface BrokerAccountEntity extends CommonBrokerAccountEntity {
	type: BrokerAccountTypeEntity,
	currency: CurrencyEntity,
	broker: BrokerEntity,
}

export interface BrokerAccountEntityResponse extends CommonBrokerAccountEntity{
	type: BrokerAccountTypeEntity,
	currency: CurrencyEntity,
	broker: BrokerEntity,
}