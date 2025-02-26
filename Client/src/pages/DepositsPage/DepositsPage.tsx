import { Fragment, useEffect, useRef, useState } from "react";
import { DepositEntity } from "../../models/DepositEntity";
import DepositModal, { DepositModalRef } from "../../modals/DepositModal/DepositModal";
import { AddIcon } from "@chakra-ui/icons";
import { createDeposit, getDeposits } from "../../api/depositApi";
import { Button, Flex, SimpleGrid } from "@chakra-ui/react";
import DepositStats from "../../components/DepositStats/DepositStats";
import Deposit from "../../components/Deposit/Deposit";

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

    return (
        <Fragment> 
            <DepositStats/>
            <Flex>
                <Button onClick={showDepositModal} leftIcon={<AddIcon/>} colorScheme='purple' size='md'>
                    Add deposit
                </Button>
            </Flex>
            <DepositModal ref={modalRef} onSaved={onDepositAdded}/>
            <SimpleGrid pt={5} pb={5} spacing={4} templateColumns='repeat(auto-fill, minmax(400px, 3fr))'>
                {
                    state.deposits.map((deposit: DepositEntity) => 
                        <Deposit onUpdated={onDepositUpdate} onDeleted={onDepositDeleted} deposit={deposit}/>
                    )
                }
            </SimpleGrid>
        </Fragment>
    )
    
}

export default DepositsPage;