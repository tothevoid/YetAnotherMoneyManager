import { Fragment, useEffect, useRef, useState } from "react";
import { DepositEntity } from "../../models/DepositEntity";
import DepositModal, { DepositModalRef } from "../../modals/DepositModal/DepositModal";
import { createDeposit, getDeposits } from "../../api/depositApi";
import { Button, Text, Flex, SimpleGrid, Slider } from "@chakra-ui/react";
import DepositStats from "../../components/DepositStats/DepositStats";
import Deposit from "../../components/Deposit/Deposit";
import { MdAdd } from "react-icons/md";
import DepositsRangeSlider from "../../components/DepositsRangeSlider/DepositsRangeSlider";
import { useTranslation } from "react-i18next";

interface Props {}

interface State {
    deposits: DepositEntity[],
    selectedMinMonths: number,
    selectedMaxMonths: number,
}

const DepositsPage: React.FC<Props> = () => {
    const [state, setState] = useState<State>({deposits: [], selectedMinMonths: 0, selectedMaxMonths: 0});
    const { t } = useTranslation();

    useEffect(() => {
        const initDeposits = async () => {
            if (!state.selectedMinMonths || !state.selectedMaxMonths) {
                return;
            }

            const deposits = await getDeposits(state.selectedMinMonths, state.selectedMaxMonths);
            setState((currentState) => {
                return {...currentState, deposits};
            });
        }
        initDeposits();
      }, [state.selectedMinMonths, state.selectedMaxMonths]);

    const modalRef = useRef<DepositModalRef>(null);

    const showDepositModal = () => {
        modalRef.current?.openModal()
    };

    const onDepositAdded = async (deposit: DepositEntity) => {
        const addedDeposit = await createDeposit(deposit);
        if (!addedDeposit) {
            return;
        }
        setState((currentState) => {
            const newDeposits = {deposits: [addedDeposit, ...state.deposits]};
            return {...currentState, deposits: newDeposits};
        });
	};

    
    const onDepositUpdate = (updatedDeposit: DepositEntity) => {
        const deposits = state.deposits.map(deposit => 
            deposit.id === updatedDeposit.id ?
                {...updatedDeposit}:
                deposit
        );
        setState((currentState) => {
            return {...currentState, deposits};
        });
    } 

    const onDepositDeleted = (deletedDeposit: DepositEntity) => {
        const deposits = state.deposits.filter(deposit => deposit.id !== deletedDeposit.id);
        setState((currentState) => {
            return {...currentState, deposits};
        });
    }

    const getAddButton = () => {
        return <Button onClick={showDepositModal} background='purple.600' size='md'>
            <MdAdd/>
            Add deposit
        </Button>
    }

    const getAddButtonWithDeposits = () => {
        return <Flex paddingTop={4}>
            {getAddButton()}
        </Flex>
    }

    const getAddButtonWithoutDeposits = () => {
        return <Flex gap={4} direction="column" alignItems="center" justifyContent="center" height="100vh" width="100%">
            <Text color={"text_primary"} fontSize="2xl">{t("deposits_page_no_deposits")}</Text>
            {getAddButton()}
        </Flex>
    }

    const onDepositsRangeChanged = async (fromMonths: number, toMonths: number) => {
        setState((currentState) => {
            return {...currentState, selectedMinMonths: fromMonths, selectedMaxMonths: toMonths};
        });
    }

    return (
        <Fragment>
            {
                state.deposits.length > 0 && state.selectedMaxMonths && state.selectedMaxMonths ?
                    <DepositStats selectedMinMonths={state.selectedMinMonths}  selectedMaxMonths={state.selectedMaxMonths}/>:
                    <Fragment/>
            }
            <DepositsRangeSlider onDepositsRangeChanged={onDepositsRangeChanged} />
            {
                state.deposits.length > 0 ?
                    getAddButtonWithDeposits():
                    getAddButtonWithoutDeposits()
            }
            <DepositModal ref={modalRef} onSaved={onDepositAdded}/>
            <SimpleGrid pt={5} pb={5} gap={6} templateColumns='repeat(auto-fill, minmax(300px, 4fr))'>
                {
                    state.deposits.map((deposit: DepositEntity) => 
                        <Deposit key={deposit.id} deposit={deposit} 
                            onUpdated={onDepositUpdate} 
                            onCloned={onDepositAdded} 
                            onDeleted={onDepositDeleted}/>
                    )
                }
            </SimpleGrid>
        </Fragment>
    )
    
}

export default DepositsPage;