import { Button, CloseButton, Dialog, Field, Input, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useEffect, useImperativeHandle, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import { SecurityFormInput, SecurityValidationSchema } from "./SecurityValidationSchema";
import { SecurityEntity } from "../../../../models/securities/SecurityEntity";
import CollectionSelect from "../../../../controls/CollectionSelect/CollectionSelect";
import { getSecurityTypes } from "../../../../api/securities/securityTypeApi";

interface SecuirtyModalProps {
    security?: SecurityEntity | null,
    onSaved: (security: SecurityEntity) => void;
};

interface State {
    securityTypes: SecurityEntity[]
}

export interface SecurityModalRef {
    openModal: () => void
}

const SecurityModal = forwardRef<SecurityModalRef, SecuirtyModalProps>((props: SecuirtyModalProps, ref)=> {
    const { open, onOpen, onClose } = useDisclosure()

    const [state, setState] = useState<State>({securityTypes: []})
        
    useEffect(() => {
        const initData = async () => {
            await requestData();
        }
        initData();
    }, []);

    const requestData = async () => {
        const securityTypes = await getSecurityTypes();
        setState((currentState) => {
            return {...currentState, securityTypes }
        })
    };

    const { register, handleSubmit, control, formState: { errors }} = useForm<SecurityFormInput>({
        resolver: zodResolver(SecurityValidationSchema),
        mode: "onBlur",
        defaultValues: {
            id: props.security?.id ?? crypto.randomUUID(),
            name: props.security?.name ?? "",
            ticker: props.security?.ticker ?? "",
            type: props.security?.type
        }
    });

    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));


    const onSubmit = (security: SecurityFormInput) => {
        props.onSaved(security as SecurityEntity);
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
                        <Dialog.Title>{t("entity_security_from_title")}</Dialog.Title>
                    </Dialog.Header>
                    <Dialog.Body pb={6}>
                        <Field.Root invalid={!!errors.name}>
                            <Field.Label>{t("entity_security_name")}</Field.Label>
                            <Input {...register("name")} autoComplete="off" placeholder='Debit card' />
                            <Field.ErrorText>{errors.name?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root invalid={!!errors.ticker}>
                            <Field.Label>{t("entity_security_ticker")}</Field.Label>
                            <Input {...register("ticker")} autoComplete="off" placeholder='NVDA' />
                            <Field.ErrorText>{errors.ticker?.message}</Field.ErrorText>
                        </Field.Root>
                        <Field.Root mt={4} invalid={!!errors.type}>
                            <Field.Label>{t("entity_security_type")}</Field.Label>
                            <CollectionSelect name="type" control={control} placeholder="Select type"
                                collection={state.securityTypes} 
                                labelSelector={(currency => currency.name)} 
                                valueSelector={(currency => currency.id)}/>
                            <Field.ErrorText>{errors.type?.message}</Field.ErrorText>
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
export default SecurityModal;