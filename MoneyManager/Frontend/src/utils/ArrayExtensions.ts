export const insertByPredicate = <T>(array: T[], newElement: T, predicate: (elm: T) => Boolean): T[] => {
    const index = array.findIndex(predicate);
    if (index !== -1){
        array.splice(index, 0, {...newElement})
    } else {
        array.push(newElement)
    }
    return array;
}