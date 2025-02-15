import './Fund.scss'
import { FundEntity } from '../../models/FundEntity';
import { AlertDialog, AlertDialogBody, AlertDialogContent, AlertDialogFooter, AlertDialogHeader, AlertDialogOverlay, Button, Card, CardBody, Flex, Stack, Text, useDisclosure } from '@chakra-ui/react';
import { currency } from '../../constants/currency';
import { DeleteIcon, EditIcon } from '@chakra-ui/icons';
import React, { Fragment, useRef } from 'react';
import FundModal from '../../modals/FundModal/FundModal';

type Props = {
    fund: FundEntity,
    onDeleteCallback: (fund: FundEntity) => void,
    onEditCallback: (fund: FundEntity) => void
}

const Fund = (props: Props) => {
    const {name, balance} = props.fund;

    const { isOpen, onOpen, onClose } = useDisclosure()
    const cancelRef = React.useRef(null)

    const modalRef = useRef(null);

    const onEdit = () => {
        modalRef.current?.openModal()
    };
   

    const onDeleteClicked = () => {
        onClose(); 
        props.onDeleteCallback(props.fund);
    } 

    return <Fragment>
        <Card>
            <CardBody boxShadow={"sm"} _hover={{ boxShadow: "md" }} >
                <Flex justifyContent="space-between" alignItems="center">
                    <Stack>
                        <Text fontWeight={600}>{name}</Text>
                        <Text fontWeight={700}>{balance}{currency.rub}</Text>
                    </Stack>
                    <div>
                        <Button background={'white'} size={'sm'} onClick={() => onEdit()}><EditIcon/></Button>
                        <Button background={'white'} size={'sm'} onClick={onOpen}>
                            <DeleteIcon color={"red.600"}/>
                        </Button>
                    </div>
                </Flex>
            </CardBody>
        </Card>

        <AlertDialog
            isOpen={isOpen}
            leastDestructiveRef={cancelRef}
            onClose={onClose}>
            <AlertDialogOverlay>
                <AlertDialogContent>
                    <AlertDialogHeader fontSize='lg' fontWeight='bold'>
                        Delete fund
                    </AlertDialogHeader>
                    <AlertDialogBody>
                        Are you sure? You can't undo this action afterwards.
                    </AlertDialogBody>

                    <AlertDialogFooter>
                    <Button ref={cancelRef} onClick={onClose}>
                        Cancel
                    </Button>
                    <Button colorScheme='red' onClick={onDeleteClicked} ml={3}>
                        Delete
                    </Button>
                    </AlertDialogFooter>
                </AlertDialogContent>
            </AlertDialogOverlay>
        </AlertDialog>
        <FundModal fund={props.fund} ref={modalRef} onSaved={props.onEditCallback}></FundModal>
    </Fragment>
};

export default Fund;