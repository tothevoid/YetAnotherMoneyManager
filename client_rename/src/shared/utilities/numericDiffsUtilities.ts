export const calculateDiff = (currentValue: number, initialValue: number) => {
    const profitAndLoss = currentValue - initialValue;
    const profitAndLossPercentage = initialValue ? 
        profitAndLoss / initialValue * 100:
        0;
    
    const color = profitAndLoss > 0 ?
        "green.600":
        "red.600";

    return {profitAndLoss, profitAndLossPercentage, color}
}
