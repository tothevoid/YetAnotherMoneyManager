import React, { useEffect, useState } from 'react';
import { Box, Flex } from '@chakra-ui/react';
import { useBrokerAccountFundTransfers } from '../../hooks/useBrokerAccountFundTransfers';
import BrokerAccountFundTransfer from '../BrokerAccountFundTransfer/BrokerAccountFundTransfer';
import { BrokerAccountFundTransferEntity } from '../../../../models/brokers/BrokerAccountFundTransfer';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { useEntityModal } from '../../../../shared/hooks/useEntityModal';
import { useTranslation } from 'react-i18next';
import BrokerAccountFundTransferModal, { CreateBrokerAccountFundTransferContext, EditBrokerAccountFundTransferContext } from '../../../BrokerAccounts/modals/BrokerAccountFundTransferModal/BrokerAccountFundTransferModal';
import { Nullable } from '../../../../shared/utilities/nullable';
import AddButton from '../../../../shared/components/AddButton/AddButton';
import { createBrokerAccountFundsTransfer, updateBrokerAccountFundsTransfer } from '../../../../api/brokers/BrokerAccountFundsTransferApi';
import { ActiveEntityMode } from '../../../../shared/enums/activeEntityMode';

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
        modalRef,
        activeEntity,
        confirmModalRef,
        onDeleteClicked,
        onAddClicked,
        onEditClicked,
        onActionEnded,
        mode
    } = useEntityModal<BrokerAccountFundTransferEntity>();

    const onDeleteConfirmed = async () => {
       	if (!activeEntity) {
			throw new Error("Deleted entity is not set")
		}

		await deleteFundTransferEntity(activeEntity);
        await reloadFundTransfers();
		onActionEnded();
    }

    const [context, setContext] = useState<Nullable<CreateBrokerAccountFundTransferContext | EditBrokerAccountFundTransferContext>>(null);

    const onTransferSaved = async (transfer: BrokerAccountFundTransferEntity) => {
        if (mode === ActiveEntityMode.Add) {
            await createBrokerAccountFundsTransfer(transfer);
        } else if (mode === ActiveEntityMode.Edit) {
            await updateBrokerAccountFundsTransfer(transfer);
        }

        onActionEnded();
        await reloadFundTransfers();
    };

	useEffect(() => {
		const context = activeEntity ?
			{ brokerAccountFundTransfer: activeEntity } as EditBrokerAccountFundTransferContext:
			{ brokerAccountId: props.brokerAccountId } as CreateBrokerAccountFundTransferContext;
		setContext(context);
	}, [props.brokerAccountId, activeEntity]);

    useEffect(() => {
        if (props?.onDataChanged){
            props.onDataChanged();
        }
    }, [props.onDataChanged, fundTransfers]);

    const {t} = useTranslation();

    return <Box>
        <Flex alignItems="center" gapX={5}>
			<AddButton buttonTitle={t("broker_account_page_transfer_button")} onClick={onAddClicked}/>
		</Flex>
        <Box>
        {
            fundTransfers.map((fundTransfer: BrokerAccountFundTransferEntity) => 
                <BrokerAccountFundTransfer key={fundTransfer.id}
                    onEditClicked={onEditClicked}
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
        {context && <BrokerAccountFundTransferModal modalRef={modalRef} context={context} onSaved={onTransferSaved}  />}
    </Box>
}

export default BrokerAccountFundTransfersList;