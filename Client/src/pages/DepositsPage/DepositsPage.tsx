import { Fragment, useEffect, useRef, useState } from "react";
import { DepositEntity } from "../../models/DepositEntity";
import DepositModal, { DepositModalRef } from "../../modals/DepositModal/DepositModal";
import { createDeposit, getDeposits } from "../../api/depositApi";
import { Button, Text, Flex, SimpleGrid, Slider } from "@chakra-ui/react";
import DepositStats from "../../components/DepositStats/DepositStats";
import Deposit from "../../components/Deposit/Deposit";
import { MdAdd } from "react-icons/md";

interface Props {}

interface State {
    deposits: DepositEntity[]
}

const DepositsPage: React.FC<Props> = () => {
    const [state, setState] = useState<State>({deposits: []});

    useEffect(() => {
        const initDeposits = async () => {
            const deposits = await getDeposits();
            setState({deposits});
        }
        initDeposits();
      }, []);

    const modalRef = useRef<DepositModalRef>(null);

    const showDepositModal = () => {
        modalRef.current?.openModal()
    };

    const onDepositAdded = async (deposit: DepositEntity) => {
        const addedDeposit = await createDeposit(deposit);
        if (!addedDeposit) {
            return;
        }
        setState({deposits: [addedDeposit, ...state.deposits]})
	};

    
    const onDepositUpdate = (updatedDeposit: DepositEntity) => {
        const deposits = state.deposits.map(deposit => 
            deposit.id === updatedDeposit.id ?
                {...updatedDeposit}:
                deposit
        );
        setState({deposits});
    } 

    const onDepositDeleted = (deletedDeposit: DepositEntity) => {
        const deposits = state.deposits.filter(deposit => deposit.id !== deletedDeposit.id);
        setState({deposits});
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
            <Text fontSize="2xl">There is no deposits yet.</Text>
            {getAddButton()}
        </Flex>
    }

    return (
        <Fragment>
            {
                state.deposits.length > 0 ?
                    <DepositStats/>:
                    <Fragment/>
            }
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