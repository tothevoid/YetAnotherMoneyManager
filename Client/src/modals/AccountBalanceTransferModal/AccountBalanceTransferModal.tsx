import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useEffect, useImperativeHandle, useState } from "react"
import { AccountEntity } from "../../models/accounts/AccountEntity";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { AccountFormInput, AccountBalanceTransferModalValidationSchema } from "./AccountBalanceTransferModalValidationSchema";
import { useTranslation } from "react-i18next";
import { getAccounts, transferBalance } from "../../api/accountApi";
import CollectionSelect from "../../controls/CollectionSelect/CollectionSelect";

type AccountProps = {
    from?: AccountEntity | null,
    onTransfered: () => void;
};

type State = {
    accounts: AccountEntity[]
}

export type Transfer = {
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


    const onSubmit = async (transfer: Transfer) => {
        const isTransfered = await transferBalance(transfer)

        if (!isTransfered) {
            return;
        }
        
        props.onTransfered();
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
                        <CollectionSelect name="from" control={control} placeholder="Select sender account"
                            collection={state.accounts} 
                            labelSelector={(account => account.name)} 
                            valueSelector={(account => account.id)}/>
                        <Field.ErrorText>{errors.from?.message}</Field.ErrorText>
                    </Field.Root>

                    <Field.Root mt={4} invalid={!!errors.to}>
                        <Field.Label>{t("account_balance_transfer_modal_to")}</Field.Label>
                        <CollectionSelect name="to" control={control} placeholder="Select recepient account"
                            collection={state.accounts} 
                            labelSelector={(account => account.name)} 
                            valueSelector={(account => account.id)}/>
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