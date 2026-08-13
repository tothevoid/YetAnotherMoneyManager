import { Box, Button, Icon, Input } from "@chakra-ui/react";
import { useEffect, useMemo, useRef, useState } from "react";
import { MdAdd, MdDelete } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import { BrokerEntity } from "../../../../models/brokers/BrokerEntity";
import { createBroker, deleteBroker, getBrokers, updateBroker } from "../../../../api/brokers/brokerApi";
import BrokerModal from "../../modals/BrokerModal/BrokerModal";
import { BaseModalRef } from "../../../../shared/utilities/modalUtilities";
import DataTable, { ColumnDef } from "../../../../shared/components/DataTable/DataTable";

interface State {
    brokers: BrokerEntity[],
    hasChanges: boolean,
    currentBrokerId: string | null
}

const BrokersTable: React.FC = () => {
    const [state, setState] = useState<State>({brokers: [], hasChanges: false, currentBrokerId: null});
    const [isLoading, setIsLoading] = useState(true);
    const { t } = useTranslation();

    const modalRef = useRef<BaseModalRef>(null);
    const confirmModalRef = useRef<BaseModalRef>(null);

    useEffect(() => {
        const initData = async () => { 
            setIsLoading(true);
            try {
                const brokers = await getBrokers();
                setState((currentState) => {
                    return {...currentState, brokers}
                });
            } finally {
                setIsLoading(false);
            }
        }

        initData();
    }, []);

    const onNameChanged = (brokerId: string, newValue: string) => {
        let hasChanges = false;

        const updatedBrokers = state.brokers.map((broker: BrokerEntity) => {
            if (broker.id !== brokerId || broker.name === newValue) {
                return broker;
            }

            hasChanges = true;
            return {...broker, name: newValue};
        });

        if (!hasChanges) {
            return;
        }

        setState((currentState) => {
            return {...currentState, brokers: updatedBrokers, hasChanges: true}
        })
    }

    const onCellBlur = async (brokerId: string) => {
        if (!state.hasChanges){
            return;
        }

        const broker = state.brokers.find((broker: BrokerEntity) => {
            return broker.id === brokerId;
        });
        if (!broker) {
            return;
        }

        await updateBroker({...broker});
    }
    
    const onAdd = () => {
        modalRef.current?.openModal()
    };

    const onBrokerAdded = async (broker: BrokerEntity) => {
        const createdBroker = await createBroker(broker);
        if (!createdBroker) {
            return;
        }

        setState((currentState) => {
            return {...currentState, brokers: [...currentState.brokers, createdBroker]}
        })
    };

    const onDeleteClicked = async (broker: BrokerEntity) => {
        setState((currentState) => {
            return {...currentState, currentBrokerId: broker.id}
        })
        confirmModalRef.current?.openModal()
    }

    const onDeleteConfirmed = async () => {
        const {currentBrokerId} = state;

        if (!currentBrokerId){
            return;
        }

        const isDeleted = await deleteBroker(currentBrokerId);
        
        if (!isDeleted) {
            return;
        }

        const brokers = state.brokers.filter((broker: BrokerEntity) => {
            return broker.id !== state.currentBrokerId;
        });

        setState((currentState) => {
            return {...currentState, brokers, currentBrokerId: null}
        })
    }

    const columns: ColumnDef<BrokerEntity>[] = useMemo(() => [
        {
            header: t("entity_broker_name"),
            render: (broker) => (
                <Input
                    onBlur={() => onCellBlur(broker.id)}
                    type="text"
                    value={broker.name}
                    onChange={(handler) => onNameChanged(broker.id, handler.target.value)}
                />
            )
        },
        {
            width: 10,
            render: (broker) => (
                <Button
                    borderColor="background_secondary"
                    background="button_background_secondary"
                    size={'sm'}
                    onClick={() => onDeleteClicked(broker)}
                >
                    <Icon color="card_action_icon_danger">
                        <MdDelete/>
                    </Icon>
                </Button>
            )
        }
    ], [t, state.brokers]);

    return <Box color="text_primary">
        <Box mb={4}>
            <Button background="action_primary" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("entity_broker_add")}
            </Button>
        </Box>
        <DataTable
            data={state.brokers}
            columns={columns}
            keyExtractor={(item) => item.id}
            isLoading={isLoading}
            skeletonRows={5}
        />
        <BrokerModal modalRef={modalRef} onSaved={onBrokerAdded}/>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
            title={t("brokers_delete_title")}
            message={t("modals_delete_message")}
            confirmActionName={t("modals_delete_button")}
            ref={confirmModalRef}/>
    </Box>
}

export default BrokersTable;