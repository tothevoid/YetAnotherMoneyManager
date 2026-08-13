import { Fragment, useCallback, useEffect } from "react";
import { Box, Flex, Badge, Button } from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { DebtPaymentEntity } from "../../../../models/debts/DebtPaymentEntity";
import { useDebtPayments } from "../../hooks/useDebtPayments";
import DebtPaymentModal from "../../modals/DebtPaymentModal/DebtPaymentModal";
import DebtPayment from "../DebtPayment/DebtPayment";
import { useEntityModal } from "../../../../shared/hooks/useEntityModal";
import { ConfirmModal } from "../../../../shared/modals/ConfirmModal/ConfirmModal";
import { ActiveEntityMode } from "../../../../shared/enums/activeEntityMode";
import AddButton from "../../../../shared/components/AddButton/AddButton";
import CollectionPagination from "../../../../shared/components/CollectionPagination/CollectionPagination";
import { getDebtPaymentsPagination } from "../../../../api/debts/debtPaymentApi";
import { MdClose } from "react-icons/md";

interface Props {
    onDebtPaymentsChanged: () => void;
    selectedDebtId?: string | null;
    selectedDebtName?: string | null;
    onClearDebtFilter?: () => void;
}

const DebtsPaymentsList: React.FC<Props> = ({
    onDebtPaymentsChanged,
    selectedDebtId,
    selectedDebtName,
    onClearDebtFilter
}) => {
    const { t } = useTranslation();

    const { 
        activeEntity,
        modalRef,
        confirmModalRef,
        onAddClicked,
        onEditClicked,
        onDeleteClicked,
        mode,
        onActionEnded
    } = useEntityModal<DebtPaymentEntity>();

    const {
        debtPayments,
        createDebtPaymentEntity,
        updateDebtPaymentEntity,
        deleteDebtPaymentEntity,
        setDebtPaymentsQueryParameters
    } = useDebtPayments({ pageIndex: 1, recordsQuantity: -1, debtId: selectedDebtId || undefined });

    useEffect(() => {
        setDebtPaymentsQueryParameters((prev) => ({
            ...prev,
            pageIndex: 1,
            debtId: selectedDebtId || undefined
        }));
    }, [selectedDebtId, setDebtPaymentsQueryParameters]);

    const onDebtPaymentSaved = async (createdDebtPayment: DebtPaymentEntity) => {
        if (mode === ActiveEntityMode.Add) {
            await createDebtPaymentEntity(createdDebtPayment);
        } else if (mode === ActiveEntityMode.Edit) {
            await updateDebtPaymentEntity(createdDebtPayment);
        }
        onDebtPaymentsChanged();
        onActionEnded();
    };

    const onDeleteConfirmed = async () => {
		if (!activeEntity) {
            throw new Error("Deleted entity is not set");
        }

        await deleteDebtPaymentEntity(activeEntity);
        onDebtPaymentsChanged();
		onActionEnded();
    };

    const getPagination = useCallback(() => {
        return getDebtPaymentsPagination(selectedDebtId || undefined);
    }, [selectedDebtId]);

    const onPageChanged = async (recordsQuantity: number, pageIndex: number) => {
		setDebtPaymentsQueryParameters({ recordsQuantity, pageIndex, debtId: selectedDebtId || undefined });
	};

    return (
        <Fragment>
            <Flex justifyContent="space-between" alignItems="center" my={3}>
                <AddButton buttonTitle={t("debts_page_add_payment")} onClick={onAddClicked} />

                {selectedDebtName && (
                    <Flex alignItems="center" gap={2}>
                        <Badge colorPalette="blue" px={3} py={1} borderRadius="full" fontSize="xs">
                            {t("debts_payments_filter_debt", { name: selectedDebtName })}
                        </Badge>
                        {onClearDebtFilter && (
                            <Button size="xs" variant="ghost" onClick={onClearDebtFilter} color="text_primary">
                                <MdClose /> {t("debts_payments_filter_clear")}
                            </Button>
                        )}
                    </Flex>
                )}
            </Flex>

            <Box>
                {debtPayments.map((payment: DebtPaymentEntity) => (
                    <DebtPayment
                        key={payment.id}
                        debtPayment={payment}
                        onEditClicked={onEditClicked}
                        onDeleteClicked={onDeleteClicked}
                    />
                ))}
            </Box>

            <CollectionPagination key={selectedDebtId || "all"} getPaginationConfig={getPagination} onPageChanged={onPageChanged} />

            <ConfirmModal
                onConfirmed={onDeleteConfirmed}
                title={t("debt_payment_modal_title")}
                message={t("modals_delete_message")}
                confirmActionName={t("modals_delete_button")}
                ref={confirmModalRef}
            />
            <DebtPaymentModal debtPayment={activeEntity} modalRef={modalRef} onSaved={onDebtPaymentSaved} />
        </Fragment>
    );
};

export default DebtsPaymentsList;