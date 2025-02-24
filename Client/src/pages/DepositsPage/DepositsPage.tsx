import { Fragment, ReactNode, useEffect, useRef, useState } from "react";
import { DepositEntity } from "../../models/DepositEntity";
import DepositModal, { DepositModalRef } from "../../modals/DepositModal/DepositModal";
import { AddIcon, Button, Card, Container } from "@chakra-ui/icons";
import { createDeposit, getDeposits } from "../../api/depositApi";

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

    const onDepositAdded = async (deposit: DepositEntity) => {
        const addedDeposit = await createDeposit(deposit);
        if (!addedDeposit) {
            return;
        }
        setState({deposits: [addedDeposit, ...state.deposits]})
	};

    const modalRef = useRef<DepositModalRef>(null);

    const onAdd = () => {
        modalRef.current?.openModal()
    };

    return (
        <Container>
            <Button onClick={onAdd} leftIcon={<AddIcon/>} colorScheme='purple' size='md'>
                Add deposit
            </Button>
            <DepositModal ref={modalRef} onSaved={onDepositAdded}/>
            <Container>
                {
                    state.deposits.map(deposit => {
                        return <Card>
                            {deposit.name}
                        </Card>
                    })
                }
            </Container>
        </Container>
    )
    
}

export default DepositsPage;