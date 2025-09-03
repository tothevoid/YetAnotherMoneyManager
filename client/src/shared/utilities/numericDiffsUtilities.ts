import { formatMoneyByCurrencyCulture } from "./formatters/moneyFormatter";
import { Nullable } from "./nullable";

export const calculateDiff = (currentValue: number, initialValue: number, currency: Nullable<string> = null) => {
    const profitAndLoss = currentValue - initialValue;
    const profitAndLossPercentage = initialValue ? 
        profitAndLoss / initialValue * 100:
        0;
    
    const color = profitAndLoss > 0 ?
        "green.600":
        "red.600";

    const formattedProfitAndLoss = currency ?
        formatMoneyByCurrencyCulture(profitAndLoss, currency):
        profitAndLoss.toFixed(2)

    return {profitAndLoss: formattedProfitAndLoss, profitAndLossPercentage: profitAndLossPercentage.toFixed(2), color}
}
