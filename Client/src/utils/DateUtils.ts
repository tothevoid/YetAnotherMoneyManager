export const getMonthByIndex = (index: number) => 
    getMonthsNames()[index - 1];

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

export const getCurrentDate = (): string => {
    const date = new Date();
    return new Date(date.getTime() - (date.getTimezoneOffset() * 60000)).toISOString().substr(0, 10);
}