import React from 'react';
import { Button, Icon } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { MdFlashOn } from 'react-icons/md';

interface ExchangeRateHintProps {
    marketRate?: number | null;
    currency?: string;
    onApply?: () => void;
}

export const ExchangeRateHint: React.FC<ExchangeRateHintProps> = ({
    marketRate,
    currency,
    onApply,
}) => {
    const { t } = useTranslation();

    if (!marketRate || !currency) {
        return null;
    }

    return (
        <Button
            size="xs"
            variant="subtle"
            backgroundColor="background_secondary"
            color="action_primary"
            onClick={onApply}
            title={t("currency_transaction_apply_rate")}
            gap={1}
        >
            <Icon fontSize="xs">
                <MdFlashOn />
            </Icon>
            {t("currency_transactions_current_rate")}: {marketRate} {currency}
        </Button>
    );
};

export default ExchangeRateHint;
