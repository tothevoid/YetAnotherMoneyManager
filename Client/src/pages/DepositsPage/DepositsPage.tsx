import { Fragment, useEffect, useRef, useState } from "react";
import { DepositEntity } from "../../models/DepositEntity";
import DepositModal, { DepositModalRef } from "../../modals/DepositModal/DepositModal";
import { AddIcon, DeleteIcon, EditIcon, } from "@chakra-ui/icons";

import { createDeposit, getDeposits } from "../../api/depositApi";
import { formatMoney } from "../../formatters/moneyFormatter";
import { CardBody, Button, Card, Container,  Flex, Stack, Text, SimpleGrid } from "@chakra-ui/react";
import { formatDate } from "../../formatters/dateFormatter";

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
        <Fragment>
            <Button onClick={onAdd} leftIcon={<AddIcon/>} colorScheme='purple' size='md'>
                Add deposit
            </Button>
            <DepositModal ref={modalRef} onSaved={onDepositAdded}/>
            <SimpleGrid pt={5} pb={5} spacing={4} templateColumns='repeat(auto-fill, minmax(400px, 3fr))'>
                {
                    state.deposits.map((deposit: DepositEntity) => {
                        return <Card>
                            <CardBody boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                                <Flex justifyContent="space-between" alignItems="center">
                                    <Stack>
                                        <Text fontWeight={600}>{deposit.name}</Text>
                                        <Text fontWeight={600}>{deposit.percentage}%</Text>
                                        <Text fontWeight={700}>{formatMoney(deposit.initialAmount)}</Text>
                                        <Text fontWeight={600}>{`${formatDate(deposit.from)} - ${formatDate(deposit.to)}`}</Text>
                                    </Stack>
                                    <div>
                                        <Button background={'white'} size={'sm'}><EditIcon/></Button>
                                        <Button background={'white'} size={'sm'}>
                                            <DeleteIcon color={"red.600"}/>
                                        </Button>
                                    </div>
                                </Flex>
                            </CardBody>
                        </Card>
                    })
                }
            </SimpleGrid>
        </Fragment>
    )
    
}

export default DepositsPage;