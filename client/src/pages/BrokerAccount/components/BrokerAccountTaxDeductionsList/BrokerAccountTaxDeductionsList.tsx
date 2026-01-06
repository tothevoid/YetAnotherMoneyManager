import React, { useEffect, useState } from 'react';
import { Box, Flex } from '@chakra-ui/react';
import { useBrokerAccountTaxDeductions } from '../../hooks/useBrokerAccountTaxDeductions';
import { ConfirmModal } from '../../../../shared/modals/ConfirmModal/ConfirmModal';
import { useEntityModal } from '../../../../shared/hooks/useEntityModal';
import { useTranslation } from 'react-i18next';
import { Nullable } from '../../../../shared/utilities/nullable';
import AddButton from '../../../../shared/components/AddButton/AddButton';
import { ActiveEntityMode } from '../../../../shared/enums/activeEntityMode';
import BrokerAccountTaxDeduction from '../BrokerAccountTaxDeduction/BrokerAccountTaxDeduction';
import { BrokerAccountTaxDeductionEntity } from '../../../../models/brokers/BrokerAccountTaxDeductionEntity';
import BrokerAccountTaxDeductionModal, { CreateBrokerAccountTaxDeductionContext, EditBrokerAccountTaxDeductionContext } from '../../../BrokerAccounts/modals/BrokerAccountTaxDeductionModal/BrokerAccountTaxDeductionModal';

interface Props {
    brokerAccountId: string,
    onDataChanged?: () => void
}

const BrokerAccountTaxDeductionsList: React.FC<Props> = (props) => {
    const dataChangedHandler = () => {
        if (props?.onDataChanged){
            props.onDataChanged();
        }
    }

    const {
        taxDeductions,
        deleteTaxDeductionEntity,
        reloadTaxDeductions,
        createTaxDeductionEntity,
        updateTaxDeductionEntity
    } = useBrokerAccountTaxDeductions(dataChangedHandler);

    const { 
        modalRef,
        activeEntity,
        confirmModalRef,
        onDeleteClicked,
        onAddClicked,
        onEditClicked,
        onActionEnded,
        mode,
    } = useEntityModal<BrokerAccountTaxDeductionEntity>();

    const onDeleteConfirmed = async () => {
        if (!activeEntity) {
            throw new Error("Deleted entity is not set")
        }

        await deleteTaxDeductionEntity(activeEntity);
        await reloadTaxDeductions();
        onActionEnded();
    }

    const [context, setContext] = useState<Nullable<CreateBrokerAccountTaxDeductionContext | EditBrokerAccountTaxDeductionContext>>(null);

    const onTransferSaved = async (deduction: BrokerAccountTaxDeductionEntity) => {
        if (mode === ActiveEntityMode.Add) {
            await createTaxDeductionEntity(deduction);
        } else if (mode === ActiveEntityMode.Edit) {
            await updateTaxDeductionEntity(deduction);
        }

        onActionEnded();
        await reloadTaxDeductions();
    };

    useEffect(() => {
        const context = activeEntity ?
            { taxDeduction: activeEntity } as EditBrokerAccountTaxDeductionContext:
            { brokerAccountId: props.brokerAccountId } as CreateBrokerAccountTaxDeductionContext;
        setContext(context);
    }, [props.brokerAccountId, activeEntity]);

    const {t} = useTranslation();

    return <Box>
        <Flex alignItems="center" gapX={5}>
            <AddButton buttonTitle={t("broker_account_tax_deduction_modal_deduction_button")} onClick={onAddClicked}/>
        </Flex>
        <Box>
        {
            taxDeductions.map((taxDeduction: BrokerAccountTaxDeductionEntity) => 
                <BrokerAccountTaxDeduction key={taxDeduction.id}
                    onEditClicked={onEditClicked}
                    onDeleteClicked={onDeleteClicked}
                    taxDeduction={taxDeduction}
                />)
        }
        </Box>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("entity_broker_account_tax_deduction_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
        {context && <BrokerAccountTaxDeductionModal modalRef={modalRef} context={context} onSaved={onTransferSaved}  />}
    </Box>
}

export default BrokerAccountTaxDeductionsList;