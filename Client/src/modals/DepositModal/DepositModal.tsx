import "./DepositModal.scss"

import { forwardRef, useImperativeHandle } from 'react'
import DatePicker from "react-datepicker";
import { FormControl, Button, FormLabel, Input, Modal, ModalBody, ModalCloseButton, ModalContent, 
    ModalFooter, ModalHeader, ModalOverlay, useDisclosure, Flex, FormErrorMessage} from '@chakra-ui/react';
import { DepositEntity } from '../../models/DepositEntity';
import { zodResolver } from '@hookform/resolvers/zod';
import { Controller, useForm } from "react-hook-form";
import depositValidationSchema, { DepositFormInput } from "./DepositValidationSchema";

type Props = {
    deposit?: DepositEntity | null,
    onSaved: (deposit: DepositEntity) => void
}

export interface DepositModalRef {
    openModal: () => void
}

const DepositModal = forwardRef<DepositModalRef, Props>((props: Props, ref)=> {
    const { isOpen, onOpen, onClose } = useDisclosure();
    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));

    const { register, control, handleSubmit, formState: { errors }} = useForm<DepositFormInput>({
        resolver: zodResolver(depositValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.deposit?.id ?? crypto.randomUUID(),
            name: props.deposit?.name ?? "",
            from: props.deposit?.from ?? new Date(),
            to: props.deposit?.to ?? new Date(),
            percentage: props.deposit?.percentage ?? 0,
            initialAmount: props.deposit?.initialAmount ?? 0,
        }
    });

    const onSubmit = (deposit: DepositFormInput) => {
        props.onSaved(deposit as DepositEntity);
        onClose();
    }


    return <Modal isOpen={isOpen} onClose={onClose}>
        <ModalOverlay />
        <ModalContent as="form" onSubmit={handleSubmit(onSubmit)}>
            <ModalHeader>Deposit</ModalHeader>
            <ModalCloseButton />
            <ModalBody pb={6}>
            <FormControl isInvalid={!!errors.name}>
                <FormLabel>Name</FormLabel>
                <Input {...register("name")} autoComplete="off" placeholder='Deposit name' />
                <FormErrorMessage>{errors.name?.message}</FormErrorMessage>
            </FormControl>
            <FormControl isInvalid={!!errors.percentage} mt={4}>
                <FormLabel>Percentage</FormLabel>
                <Input {...register("percentage", { valueAsNumber: true })} type="number" placeholder='10' />
                <FormErrorMessage>{errors.percentage?.message}</FormErrorMessage>
            </FormControl>
            <FormControl isInvalid={!!errors.initialAmount} mt={4}>
                <FormLabel>Initial amount</FormLabel>
                <Input {...register("initialAmount", { valueAsNumber: true })} type='number' placeholder='10' />
                <FormErrorMessage>{errors.initialAmount?.message}</FormErrorMessage>
            </FormControl>
            <Flex gap={4} direction="row">
                <FormControl isInvalid={!!errors.from} mt={4}>
                    <FormLabel>Starts</FormLabel>
                    <Controller
                        name="from"
                        control={control}
                        render={({ field: {onChange, value} }) => (
                            <DatePicker
                            selected={value}
                            onChange={onChange}
                            dateFormat="dd.MM.yyyy"
                            placeholderText="Select from date"
                            customInput={<Input/>}/>
                        )}
                        />
                    <FormErrorMessage>{errors.from?.message}</FormErrorMessage>
                </FormControl>
                <FormControl isInvalid={!!errors.to} mt={4}>
                    <FormLabel>Ends</FormLabel>
                    {/* TODO Fix date field duplication */}
                    <Controller
                        name="to"
                        control={control}
                        render={({ field: {onChange, value} }) => (
                            <DatePicker
                            selected={value}
                            onChange={onChange}
                            dateFormat="dd.MM.yyyy"
                            placeholderText="Select to date"
                            customInput={<Input/>}/>
                        )}
                        />
                    <FormErrorMessage>{errors.to?.message}</FormErrorMessage>
                </FormControl>
            </Flex>
            </ModalBody>
            <ModalFooter>
                <Button type="submit" colorScheme='purple' mr={3}>Save</Button>
                <Button onClick={onClose}>Cancel</Button>
            </ModalFooter>
        </ModalContent>
    </Modal>
})


export default DepositModal;