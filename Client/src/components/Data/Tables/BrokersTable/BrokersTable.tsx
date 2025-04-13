import { Box, Button, Icon, Input, Table } from "@chakra-ui/react";
import { useEffect, useRef, useState } from "react";
import { MdAdd, MdDelete } from "react-icons/md";
import { useTranslation } from "react-i18next";
import { ConfirmModal, ConfirmModalRef } from "../../../../modals/ConfirmModal/ConfirmModal";
import { BrokerEntity } from "../../../../models/brokers/BrokerEntity";
import { createBroker, deleteBroker, getBrokers, updateBroker } from "../../../../api/brokers/brokerApi";
import BrokerModal, { BrokerModalRef } from "../../../brokers/modals/BrokerModal/BrokerModal";

interface Props {
}

interface State {
    brokers: BrokerEntity[],
    hasChanges: boolean,
    currentBrokerId: string | null
}

const BrokersTable: React.FC<Props> = () => {
    const [state, setState] = useState<State>({brokers: [], hasChanges: false, currentBrokerId: null});
    const { t } = useTranslation();
    const modalRef = useRef<BrokerModalRef>(null);
    const confirmModalRef = useRef<ConfirmModalRef>(null);

    useEffect(() => {
        const initData = async () => { 
            const brokers = await getBrokers();
            setState((currentState) => {
                return {...currentState, brokers}
            })
        }

        initData();
    }, []);

    const onCellChanged = (brokerId: string, propertyName: string, newValue: any) => {
        let hasChanges = false;

        const updatedBrokers = state.brokers.map((broker: BrokerEntity) => {
            if (broker.id !== brokerId) {
                return broker;
            }

            const currentValue = broker[propertyName];
            if (currentValue === newValue) {
                return broker;
            }

            hasChanges = true;
            return {...broker, [propertyName]: newValue};
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

    return <Box color="text_primary">
        <Table.Root>
            <Table.Header>
                <Table.Row border="none" bg="none" color="text_primary">
                    <Table.ColumnHeader color="text_primary">Name</Table.ColumnHeader>
                    <Table.ColumnHeader color="text_primary">Delete</Table.ColumnHeader>
                </Table.Row>
            </Table.Header>
            <Table.Body>
                {
                    state.brokers.map((broker: BrokerEntity) => {
                        return <Table.Row border="none" bg="none" color="text_primary" key={broker.id}>
                            <Table.Cell>
                                <Input onBlur={() => onCellBlur(broker.id)} type="text" value={broker.name}
                                    onChange={(handler) => onCellChanged(broker.id, "name", handler.target.value)}>
                                </Input>
                            </Table.Cell>
                            <Table.Cell width={10}>
                                <Button borderColor="background_secondary" background="button_background_secondary" size={'sm'} onClick={() => onDeleteClicked(broker)}>
                                    <Icon color="card_action_icon_danger">
                                        <MdDelete/>
                                    </Icon>
                                </Button>
                            </Table.Cell>
                        </Table.Row>
                    })
                }
            </Table.Body>
        </Table.Root>
        <Box padding={4}>
            <Button background="purple.600" onClick={onAdd}>
                <Icon size='md'>
                    <MdAdd/>
                </Icon>
                {t("entity_broker_add")}
            </Button>
        </Box>
        <BrokerModal ref={modalRef} onSaved={onBrokerAdded}></BrokerModal>
        <ConfirmModal onConfirmed={onDeleteConfirmed}
                    title={t("brokers_delete_title")}
                    message={t("modals_delete_message")}
                    confirmActionName={t("modals_delete_button")}
                    ref={confirmModalRef}>
        </ConfirmModal>
    </Box>
}

export default BrokersTable;