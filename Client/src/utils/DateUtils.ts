export const getMonthByIndex = (index: number) => {
    return getMonthsNames()[index - 1];
}

export const getMonthsNames = () => 
    [
        'January',
        'February',
        'March',
        'April',
        'May',
        'June',
        'July',
        'August',
        'September',
        'October',
        'November',
        'December'
    ];