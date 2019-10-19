export const getMonthByIndex = (index: number) => 
    getMonthsNames()[index - 1];

export const convertToInputDate = (date: Date) => {
    const year = date.getFullYear().toString();
    const month = (date.getMonth() + 1).toString().padStart(2, "0");
    const day = date.getDate().toString().padStart(2, "0");
    return `${year}-${month}-${day}`;
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

export const getCurrentDate = (): string => {
    const date = new Date();
    return new Date(date.getTime() - (date.getTimezoneOffset() * 60000)).toISOString().substr(0, 10);
}