import { Button, CloseButton, Dialog, Field, Portal, useDisclosure} from "@chakra-ui/react"
import { forwardRef, useEffect, useImperativeHandle, useState } from "react"
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useTranslation } from "react-i18next";
import CollectionSelect from "../../controls/CollectionSelect/CollectionSelect";
import { UserProfileFormInput, UserProfileValidationSchema } from "./UserProfileValidationSchema";
import { getCurrencies } from "../../api/currencies/currencyApi";
import { useUserProfile } from "../../contexts/UserProfileContext";
import { updateUserProfile } from "../../api/user/userProfileApi";
import { UserProfileEntity } from "../../models/user/UserProfileEntity";
import { CurrencyEntity } from "../../models/currencies/CurrencyEntity";

interface Props {}

interface State{
    currencies: CurrencyEntity[]
    languages: {key: string, value: string}[]
}

export interface UserProfileSettingsModalRef {
    openModal: () => void
}

const convertToSchemaValues = (userProfile: UserProfileEntity | null): UserProfileFormInput => {
    return {
        id: userProfile?.id,
        languageCode: languages.find((lang) => lang.value === userProfile?.languageCode) ?? languages[0],
        currency: userProfile?.currency
    }
}

const langMapping = new Map<string, string>([
    ["English", "en-US"],
    ["Русский", "ru-RU" ],
]);

const languages = [...langMapping.entries()].map(([key, value]) => {return {key, value}});

const UserProfileSettingsModal = forwardRef<UserProfileSettingsModalRef, Props>((props: Props, ref)=> {
    useImperativeHandle(ref, () => ({
        openModal: onOpen,
    }));
    
    const [state, setState] = useState<State>({currencies: [], languages: languages})
    const {user, updateUser} = useUserProfile();
    const { open, onOpen, onClose } = useDisclosure();

    useEffect(() => {
        const initData = async () => {
            await initCurrencies();
        }
        initData();
    }, []);

    const initCurrencies = async () => {
        const currencies = await getCurrencies();
        setState((currentState) => {
            return {...currentState, currencies}
        })
    };

    const { reset, watch, handleSubmit, control, formState: { errors }} = useForm<UserProfileFormInput>({
        resolver: zodResolver(UserProfileValidationSchema),
        mode: "onBlur",
        defaultValues: convertToSchemaValues(user)
    });

    useEffect(() => {
        if (open && user) {
            reset(convertToSchemaValues(user));
        }
    }, [open, user]);

    const onSubmit = async (userProfileForm: UserProfileFormInput) => {
        const userProfile: UserProfileEntity = {
             ...userProfileForm,
            languageCode: userProfileForm.languageCode.value
        }

        await updateUserProfile(userProfile);
        updateUser(userProfile);
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
                    <Field.Root mt={4} invalid={!!errors.currency}>
                        <Field.Label>Currency</Field.Label>
                        <CollectionSelect name="currency" control={control} placeholder="Select currency account"
                            collection={state.currencies} 
                            labelSelector={(currency => currency.name)} 
                            valueSelector={(currency => currency.id)}/>
                        <Field.ErrorText>{errors.currency?.message}</Field.ErrorText>
                    </Field.Root>
                    <Field.Root mt={4} invalid={!!errors.languageCode}>
                        <Field.Label>Language</Field.Label>
                        <CollectionSelect name="languageCode" control={control} placeholder="Select language"
                            collection={state.languages} 
                            labelSelector={(language => language.key)} 
                            valueSelector={(language => language.value)}/>
                        <Field.ErrorText>{errors.languageCode?.message}</Field.ErrorText>
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

export default UserProfileSettingsModal