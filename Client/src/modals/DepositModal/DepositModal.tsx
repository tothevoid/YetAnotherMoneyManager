import "./DepositModal.scss"

import React, { forwardRef, useImperativeHandle, useState } from 'react'
import DatePicker from "react-datepicker";
import { FormControl, Button, FormLabel, Input, Modal, ModalBody, ModalCloseButton, 
    ModalContent, ModalFooter, ModalHeader, ModalOverlay, useDisclosure} from '@chakra-ui/react';
import { DepositEntity } from '../../models/DepositEntity';


type Props = {
    deposit?: DepositEntity | null,
    onSaved: (deposit: DepositEntity) => void
}

export interface DepositModalRef {
    openModal: () => void
}

const DepositModal = forwardRef<DepositModalRef, Props>((props: Props, ref)=> {
    const { isOpen, onOpen, onClose } = useDisclosure()

    const getInitialState = (): DepositEntity => {    
        return {
            id: props.deposit?.id ?? crypto.randomUUID(),
            name: props.deposit?.name ?? "",
            from: props.deposit?.from ?? new Date(),
            to: props.deposit?.to ?? new Date(),
        };
    }

    const [depositModalData, setDepositModalData] = useState<DepositEntity>(getInitialState);
 
    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value, type } = e.target;
        const normalizedValue = (type === "number" || type === "price") ? parseFloat(value) : value;
        setDepositModalData((prev) => ({ ...prev, [name]: normalizedValue }));
    };

    const onDateChanged = (date: Date | null) => {
        if (!date) {
            return;
        }

        setDepositModalData((prev: DepositEntity) => ({ ...prev, date: date }));
    }

    const onDepositModalSaveClick = () => {
        props.onSaved(depositModalData);
        onClose();
    };

    return <Modal isOpen={isOpen} onClose={onClose}>
        <ModalOverlay />
        <ModalContent>
            <ModalHeader>New fund</ModalHeader>
            <ModalCloseButton />
            <ModalBody pb={6}>
            <FormControl>
                <FormLabel>Name</FormLabel>
                <Input name="name" value={depositModalData.name} onChange={handleChange} placeholder='Deposit name' />
            </FormControl>
            <FormControl mt={4}>
                <FormLabel>From</FormLabel>
                <DatePicker wrapperClassName="deposit-datepicker"
                    selected={depositModalData.from}
                    onChange={onDateChanged}
                    dateFormat="dd.MM.yyyy"
                    customInput={<Input/>}
                />
            </FormControl>
            <FormControl mt={4}>
                <FormLabel>To</FormLabel>
                <DatePicker wrapperClassName="deposit-datepicker"
                    selected={depositModalData.to}
                    onChange={onDateChanged}
                    dateFormat="dd.MM.yyyy"
                    customInput={<Input/>}
                />
            </FormControl>
            </ModalBody>
            <ModalFooter>
                <Button onClick={onDepositModalSaveClick} colorScheme='blue' mr={3}>Save</Button>
                <Button onClick={onClose}>Cancel</Button>
            </ModalFooter>
        </ModalContent>
    </Modal>
})


export default DepositModal;