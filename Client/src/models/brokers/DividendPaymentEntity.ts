import { DividendEntity } from "../securities/DividendEntity";
import { BrokerAccountEntity } from "./BrokerAccountEntity";

interface CommonDividendPaymentEntity {
	id: string,
	securitiesQuantity: number,
	tax: number
}

export interface ClientDividendPaymentEntity extends CommonDividendPaymentEntity {
	brokerAccount: BrokerAccountEntity,
	dividend: DividendEntity,
	receivedAt: Date
}

export interface ServerDividendPaymentEntity extends CommonDividendPaymentEntity {
	brokerAccountId: string,
	dividendId: string,
	receivedAt: string
}


