import React, { useEffect } from 'react';
import { Box } from '@chakra-ui/react';
import { useBrokerAccountFundTransfers } from '../../hooks/useBrokerAccountFundTransfers';
import BrokerAccountFundTransfer from '../BrokerAccountFundTransfer/BrokerAccountFundTransfer';
import { BrokerAccountFundTransferEntity } from '../../../../models/brokers/BrokerAccountFundTransfer';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { useEntityModal } from '../../../../shared/hooks/useEntityModal';
import { useTranslation } from 'react-i18next';

interface Props {
    brokerAccountId: string,
    onDataChanged?: () => void
}

const BrokerAccountFundTransfersList: React.FC<Props> = (props) => {
    const {
        fundTransfers,
        deleteFundTransferEntity,
        reloadFundTransfers
    } = useBrokerAccountFundTransfers({ brokerAccountId: props.brokerAccountId });

    const { 
        activeEntity,
        confirmModalRef,
        onDeleteClicked,
        onActionEnded
    } = useEntityModal<BrokerAccountFundTransferEntity>();

    const onDeleteConfirmed = async () => {
       	if (!activeEntity) {
			throw new Error("Deleted entity is not set")
		}

		await deleteFundTransferEntity(activeEntity);
        reloadFundTransfers();
		onActionEnded();
    }

    useEffect(() => {
        if (props?.onDataChanged){
            props.onDataChanged();
        }
    }, [props, fundTransfers]);

    const {t} = useTranslation();

    return <Box>
        <Box>
        {
            fundTransfers.map((fundTransfer: BrokerAccountFundTransferEntity) => 
                <BrokerAccountFundTransfer key={fundTransfer.id}
                    onDeleteClicked={onDeleteClicked}
                    fundTransfer={fundTransfer}
                />)
        }
        </Box>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("entity_broker_account_fund_transfer_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
    </Box>
}

export default BrokerAccountFundTransfersList;