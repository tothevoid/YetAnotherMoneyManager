import { useEffect, useRef, useState } from "react";

export const useDelayedLoading = (isLoading: boolean, delayMs = 180, minDisplayMs = 250): boolean => {
    const [showSkeleton, setShowSkeleton] = useState(false);
    const shownAtRef = useRef<number | null>(null);

    useEffect(() => {
        let delayTimer: ReturnType<typeof setTimeout> | null = null;
        let minDisplayTimer: ReturnType<typeof setTimeout> | null = null;

        if (isLoading) {
            delayTimer = setTimeout(() => {
                shownAtRef.current = Date.now();
                setShowSkeleton(true);
            }, delayMs);
        } else {
            if (shownAtRef.current) {
                const elapsed = Date.now() - shownAtRef.current;
                if (elapsed < minDisplayMs) {
                    minDisplayTimer = setTimeout(() => {
                        setShowSkeleton(false);
                        shownAtRef.current = null;
                    }, minDisplayMs - elapsed);
                } else {
                    setShowSkeleton(false);
                    shownAtRef.current = null;
                }
            } else {
                setShowSkeleton(false);
            }
        }

        return () => {
            if (delayTimer) clearTimeout(delayTimer);
            if (minDisplayTimer) clearTimeout(minDisplayTimer);
        };
    }, [isLoading, delayMs, minDisplayMs]);

    return showSkeleton;
};

export default useDelayedLoading;
