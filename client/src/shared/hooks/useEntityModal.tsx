import { useState, useRef, useCallback } from "react";
import { ActiveEntityMode } from "../enums/activeEntityMode";
import { BaseModalRef } from "../utilities/modalUtilities";

export function useEntityModal<T>() {
    const [activeEntity, setActiveEntity] = useState<T | null>(null);
    const [mode, setMode] = useState<ActiveEntityMode>(ActiveEntityMode.None);

    const modalRef = useRef<BaseModalRef>(null);
    const confirmModalRef = useRef<BaseModalRef>(null);

    const onAddClicked = useCallback(() => {
        setMode(ActiveEntityMode.Add);
        modalRef.current?.openModal();
    }, []);

    const onEditClicked = useCallback((entity: T) => {
        setActiveEntity(entity);
        setMode(ActiveEntityMode.Edit);
        modalRef.current?.openModal();
    }, []);

    const onDeleteClicked = useCallback((entity: T) => {
        setActiveEntity(entity);
        setMode(ActiveEntityMode.Delete);
        confirmModalRef.current?.openModal();
    }, []);

    const onActionEnded = useCallback(() => {
        setActiveEntity(null);
        setMode(ActiveEntityMode.None);
    }, []);

    return {
        activeEntity,
        mode,
        modalRef,
        confirmModalRef,
        onAddClicked,
        onEditClicked,
        onDeleteClicked,
        setActiveEntity,
        onActionEnded,
        setMode,
    };
}
