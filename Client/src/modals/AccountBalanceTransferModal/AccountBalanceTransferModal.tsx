import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useEffect, useImperativeHandle, useState } from "react"
import { AccountEntity } from "../../models/AccountEntity";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AccountFormInput, AccountBalanceTransferModalValidationSchema } from "./AccountBalanceTransferModalValidationSchema";
import { useTranslation } from "react-i18next";
import { Select } from "chakra-react-select";
import { getAccounts } from "../../api/accountApi";

type AccountProps = {
    from?: AccountEntity | null,
    onTransfered: (transfer: Transfer) => void;
};

type State = {
    accounts: AccountEntity[]
}

type Transfer = {
    from: AccountEntity,
    to: AccountEntity,
    balance: number
    fee: number
}

export interface TransferModalRef {
    openModal: () => void
}

const AccountBalanceTransferModal = forwardRef<TransferModalRef, AccountProps>((props: AccountProps, ref)=> {
    const [state, setState] = useState<State>({accounts: []})
    
    useEffect(() => {
        const initData = async () => {
            await initAccounts();
        }
        initData();
    }, []);

    const initAccounts = async () => {
        const accounts = await getAccounts();
        setState((currentState) => {
            return {...currentState, accounts}
        })
    };

    const { open, onOpen, onClose } = useDisclosure()

    const { register, handleSubmit, control, formState: { errors }} = useForm<AccountFormInput>({
        resolver: zodResolver(AccountBalanceTransferModalValidationSchema),
        mode: "onBlur",
        defaultValues: {
            from: props.from,
            to: null,
            balance: 0,
            fee: 0
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));


    const onSubmit = (transfer: Transfer) => {
        props.onTransfered({...transfer} as Transfer);
        onClose();
    }

    const {t} = useTranslation()

    return (
        <Dialog.Root placement="center" open={open} onEscapeKeyDown={onClose}>
          <Portal>
            <Dialog.Backdrop/>
            <Dialog.Positioner>
                <Dialog.Content as="form" onSubmit={handleSubmit(onSubmit)}>
                    <Dialog.Header>
                        <Dialog.Title>{t("account_balance_transfer_modal_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>

                    <Field.Root mt={4} invalid={!!errors.from}>
                        <Field.Label>{t("account_balance_transfer_modal_from")}</Field.Label>
                        <Controller
                            name="from"
                            control={control}
                            render={({ field }) => (
                                    <Select
                                        {...field}
                                        getOptionLabel={(e) => e.name}
                                        getOptionValue={(e) => e.id}
                                        options={state.accounts}
                                        isClearable
                                        placeholder='Select account'>
                                    </Select>
                                )}
                            />
                        <Field.ErrorText>{errors.from?.message}</Field.ErrorText>
                    </Field.Root>

                    <Field.Root mt={4} invalid={!!errors.to}>
                        <Field.Label>{t("account_balance_transfer_modal_to")}</Field.Label>
                        <Controller
                            name="to"
                            control={control}
                            render={({ field }) => (
                                    <Select
                                        {...field}
                                        getOptionLabel={(e) => e.name}
                                        getOptionValue={(e) => e.id}
                                        options={state.accounts}
                                        isClearable
                                        placeholder='Select account'>
                                    </Select>
                                )}
                            />
                        <Field.ErrorText>{errors.to?.message}</Field.ErrorText>
                    </Field.Root>
        
                    <Field.Root invalid={!!errors.balance} mt={4}>
                        <Field.Label>{t("account_balance_transfer_modal_balance")}</Field.Label>
                        <Input {...register("balance", { valueAsNumber: true })} name="balance" type="number" placeholder='10000' />
                        <Field.ErrorText>{errors.balance?.message}</Field.ErrorText>
                    </Field.Root>
                    <Field.Root invalid={!!errors.fee} mt={4}>
                        <Field.Label>{t("account_balance_transfer_modal_fee")}</Field.Label>
                        <Input {...register("fee", { valueAsNumber: true })} name="fee" type="number" placeholder='0' />
                        <Field.ErrorText>{errors.fee?.message}</Field.ErrorText>
                    </Field.Root>
                    </Dialog.Body>
                    <Dialog.Footer>
                        <Button type="submit" background='purple.600' mr={3}>{t("modals_save_button")}</Button>
                        <Button onClick={onClose}>{t("modals_cancel_button")}</Button>
                    </Dialog.Footer>
                    <Dialog.CloseTrigger asChild>
                        <CloseButton onClick={onClose} size="sm" />
                    </Dialog.CloseTrigger>
                </Dialog.Content>
            </Dialog.Positioner>
          </Portal>
        </Dialog.Root>
    )
})

export default AccountBalanceTransferModal