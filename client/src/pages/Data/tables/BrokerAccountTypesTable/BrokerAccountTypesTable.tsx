import { Box, Button, Icon, Input } from "@chakra-ui/react";
import { useEffect, useMemo, useRef, useState } from "react";
import { MdAdd, MdDelete } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import BrokerAccountTypeTypeModal from "../../modals/BrokerAccountTypeModal/BrokerAccountTypeModal";
import { createBrokerAccountType, deleteBrokerAccountType, getBrokerAccountTypes, updateBrokerAccountType } from "../../../../api/brokers/brokerAccountTypeApi";
import { BrokerAccountTypeEntity } from "../../../../models/brokers/BrokerAccountTypeEntity";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import DataTable, { ColumnDef } from "../../../../shared/components/DataTable/DataTable";

interface State {
    brokerAccountTypes: BrokerAccountTypeEntity[],
    hasChanges: boolean,
    currentBrokerAccountId: string | null
}

const BrokerAccountTypesTable: React.FC = () => {
    const [state, setState] = useState<State>({brokerAccountTypes: [], hasChanges: false, currentBrokerAccountId: null});
    const [isLoading, setIsLoading] = useState(true);
    const { t } = useTranslation();

    const modalRef = useRef<BaseModalRef>(null);
    const confirmModalRef = useRef<BaseModalRef>(null);

    useEffect(() => {
        const initData = async () => { 
            setIsLoading(true);
            try {
                const brokerAccountTypes = await getBrokerAccountTypes();
                setState((currentState) => {
                    return {...currentState, brokerAccountTypes}
                });
            } finally {
                setIsLoading(false);
            }
        }

        initData();
    }, []);

    const onNameChanged = (brokerAccountTypeId: string, newValue: string) => {
        let hasChanges = false;

        const updatedBrokerAccountTypes = state.brokerAccountTypes.map((brokerAccountType: BrokerAccountTypeEntity) => {
            if (brokerAccountType.id !== brokerAccountTypeId || brokerAccountType.name === newValue) {
                return brokerAccountType;
            }

            hasChanges = true;
            return {...brokerAccountType, name: newValue};
        });

        if (!hasChanges) {
            return;
        }

        setState((currentState) => {
            return {...currentState, brokerAccountTypes: updatedBrokerAccountTypes, hasChanges: true}
        })
    }

    const onCellBlur = async (brokerAccountTypeId: string) => {
        if (!state.hasChanges){
            return;
        }

        const brokerAccountType = state.brokerAccountTypes.find((brokerAccountType: BrokerAccountTypeEntity) => {
            return brokerAccountType.id === brokerAccountTypeId;
        });
        if (!brokerAccountType) {
            return;
        }

        await updateBrokerAccountType({...brokerAccountType});
    }
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onBrokerAccountTypeAdded = async (brokerAccountType: BrokerAccountTypeEntity) => {
        const brokerAccount = await createBrokerAccountType(brokerAccountType);
        if (!brokerAccount) {
            return;
        }

        setState((currentState) => {
            return {...currentState, brokerAccountTypes: [...currentState.brokerAccountTypes, brokerAccountType]}
        })
    };

    const onDeleteClicked = async (brokerAccountType: BrokerAccountTypeEntity) => {
        setState((currentState) => {
            return {...currentState, currentBrokerAccountId: brokerAccountType.id}
        })
        confirmModalRef.current?.openModal()
    }

    const onDeleteConfirmed = async () => {
        const {currentBrokerAccountId} = state;

        if (!currentBrokerAccountId){
            return;
        }

        const isDeleted = await deleteBrokerAccountType(currentBrokerAccountId);
        
        if (!isDeleted) {
            return;
        }

        const brokerAccountTypes = state.brokerAccountTypes.filter((brokerAccountType: BrokerAccountTypeEntity) => {
            return brokerAccountType.id !== state.currentBrokerAccountId;
        });

        setState((currentState) => {
            return {...currentState, brokerAccountTypes, currentBrokerAccountId: null}
        })
    }

    const columns: ColumnDef<BrokerAccountTypeEntity>[] = useMemo(() => [
        {
            header: t("entity_broker_account_type_name"),
            render: (brokerAccountType) => (
                <Input
                    onBlur={() => onCellBlur(brokerAccountType.id)}
                    type="text"
                    value={brokerAccountType.name}
                    onChange={(handler) => onNameChanged(brokerAccountType.id, handler.target.value)}
                />
            )
        },
        {
            width: 10,
            render: (brokerAccountType) => (
                <Button
                    borderColor="background_secondary"
                    background="button_background_secondary"
                    size={'sm'}
                    onClick={() => onDeleteClicked(brokerAccountType)}
                >
                    <Icon color="card_action_icon_danger">
                        <MdDelete/>
                    </Icon>
                </Button>
            )
        }
    ], [t, state.brokerAccountTypes]);

    return <Box color="text_primary">
        <Box mb={4}>
            <Button background="action_primary" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("entity_broker_account_type_add")}
            </Button>
        </Box>
        <DataTable
            data={state.brokerAccountTypes}
            columns={columns}
            keyExtractor={(item) => item.id}
            isLoading={isLoading}
            skeletonRows={5}
        />
        <BrokerAccountTypeTypeModal modalRef={modalRef} onSaved={onBrokerAccountTypeAdded}/>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("broker_account_types_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
    </Box>
}

export default BrokerAccountTypesTable;