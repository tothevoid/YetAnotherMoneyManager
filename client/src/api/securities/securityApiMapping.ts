import { SecurityEntityResponse, SecurityEntity, SecurityEntityRequest } from "../../models/securities/SecurityEntity";

export const prepareSecurity = (security: SecurityEntityResponse): SecurityEntity => {
    return {
        id: security.id,
        name: security.name,
        ticker: security.ticker,
        type: security.type,
        actualPrice: security.actualPrice,
        iconKey: security.iconKey,
        currency: security.currency,
        priceFetchedAt: security.priceFetchedAt && new Date(security.priceFetchedAt)
    };
}

export const prepareSecurityEntityRequest = (security: SecurityEntity): SecurityEntityRequest => {
    return {
        id: security.id,
        name: security.name,
        ticker: security.ticker,
        typeId: security.type.id,
        actualPrice: security.actualPrice,
        iconKey: security.iconKey,
        currencyId: security.currency.id,
        priceFetchedAt: security.priceFetchedAt
    };
}
