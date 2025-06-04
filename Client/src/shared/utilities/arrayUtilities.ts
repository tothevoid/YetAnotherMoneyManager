export const insertByPredicate = <T>(array: T[], newElement: T, predicate: (elm: T) => boolean): T[] => {
    const index = array.findIndex(predicate);
    if (index !== -1){
        array.splice(index, 0, {...newElement})
    } else {
        array.push(newElement)
    }
    return array;
}

export const reorderByPredicate = <T>(array: T[], newElement: T, 
    reorderPredicate: (elm: T) => boolean,
    searchPredicate: (elm: T) => boolean): T[] => {
    const filteredArray = array.filter(searchPredicate);
    return insertByPredicate(filteredArray, newElement, reorderPredicate)
}