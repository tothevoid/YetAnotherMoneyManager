import { useEffect, useRef, useState } from "react";
import { SimpleGrid, Box} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { ClientDebtEntity } from "../../models/debts/DebtEntity";
import { createDebt, getDebts } from "../../api/debts/debtApi";
import { ClientDebtPaymentEntity } from "../../models/debts/DebtPaymentEntity";
import { createDebtPayment, getDebtPayments, updateDebtPayment } from "../../api/debts/debtPaymentApi";
import Debt from "./components/Debt/Debt";
import DebtPayment from "./components/DebtPayment/DebtPayment";
import { BaseModalRef } from "../../shared/utilities/modalUtilities";
import DebtModal from "./modals/DebtModal.tsx/DebtModal";
import ShowModalButton from "../../shared/components/ShowModalButton/ShowModalButton";
import DebtPaymentModal from "./modals/DebtPaymentModal/DebtPaymentModal";

interface Props {}

interface State {
    debts: ClientDebtEntity[],
    debtPayments: ClientDebtPaymentEntity[]
}

const DebtsPage: React.FC<Props> = () => {
    const [state, setState] = useState<State>({debts: [], debtPayments: []});
    const { t } = useTranslation();

    useEffect(() => {
        const initDeposits = async () => {
            const debts = await getDebts();
            const debtPayments = await getDebtPayments();

            setState((currentState) => {
                return {...currentState, debts, debtPayments};
            });
        }
        initDeposits();
    }, []);

    const onDebtAdded = async (debt: ClientDebtEntity) => {
        const addedDebt = await createDebt(debt);
        if (!addedDebt) {
            return;
        }
        setState((currentState) => {
            return {...currentState, deposits: [addedDebt, ...state.debts]};
        });
    };

    const onDeptUpdated = async (debt: ClientDebtEntity) => {
        const debts = state.debts.map(existingDebt => 
            debt.id === existingDebt.id ?
                {...debt}:
                existingDebt
        );

        setState((currentState) => {
            return {...currentState, debts};
        });
    } 

    const onDeptDeleted = (deletedDebt: ClientDebtEntity) => {
        const debts = state.debts.filter(debt => debt.id !== deletedDebt.id);
        setState((currentState) => {
            return {...currentState, debts};
        });
    }

    const onDebtPaymentAdded = async (debtPayment: ClientDebtPaymentEntity) => {
        const addedDebtPayment = await createDebtPayment(debtPayment);
        if (!addedDebtPayment) {
            return;
        }
        setState((currentState) => {
            return {...currentState, deposits: [addedDebtPayment, ...state.debtPayments]};
        });
    };

    
    const onDebtPaymentUpdated = async (debtPayment: ClientDebtPaymentEntity) => {
        const deptPaymentUpdated = await updateDebtPayment(debtPayment);

        if (!deptPaymentUpdated) {
            return;
        }

        const debtPayments = state.debtPayments.map(existingDebtPayment => 
            debtPayment.id === existingDebtPayment.id ?
                {...debtPayment}:
                existingDebtPayment
        );

        setState((currentState) => {
            return {...currentState, debtPayments};
        });
    } 

    const onDebtPaymentDeleted = (deletedDebtPayment: ClientDebtPaymentEntity) => {
        const debtPayments = state.debtPayments
            .filter(debtPayment => debtPayment.id !== deletedDebtPayment.id);
        setState((currentState) => {
            return {...currentState, debtPayments};
        });
    }

    const modalRef = useRef<BaseModalRef>(null);
        
    const onAdd = () => {
        modalRef.current?.openModal()
    };
    
    const addDebt = async (debt: ClientDebtEntity) => {
        const createdDebt = await createDebt(debt);
        if (!createdDebt) {
            return;
        }

        onDebtAdded(createdDebt);
    };

   
    const debtPaymentModalRef = useRef<BaseModalRef>(null);
    
    const onAddDebtPayment = () => {
        debtPaymentModalRef.current?.openModal()
    };

    const addDebtPayment = async (debt: ClientDebtPaymentEntity) => {
        const createdDebtPayment = await createDebtPayment(debt);
        if (!createdDebtPayment) {
            return;
        }

        onDebtPaymentAdded(createdDebtPayment);
    };

    return (
        <Box paddingBlock={10}>
            <ShowModalButton buttonTitle={t("security_page_summary_add")} onClick={onAdd}>
                <DebtModal modalRef={modalRef} onSaved={addDebt}/>
            </ShowModalButton>
            <SimpleGrid pt={5} pb={5} gap={6} templateColumns='repeat(auto-fill, minmax(300px, 4fr))'>
                {
                    state.debts.map((debt: ClientDebtEntity) => 
                        <Debt key={debt.id} debt={debt} 
                            onEditCallback={onDeptUpdated}
                            onDeleteCallback={onDeptDeleted}/>
                    )
                }
            </SimpleGrid>
            <ShowModalButton buttonTitle={t("security_page_summary_add")} onClick={onAddDebtPayment}>
                <DebtPaymentModal modalRef={debtPaymentModalRef} onSaved={addDebtPayment}/>
            </ShowModalButton>
            <SimpleGrid pt={5} pb={5} gap={6} templateColumns='repeat(auto-fill, minmax(300px, 4fr))'>
                {
                    state.debtPayments.map((payment: ClientDebtPaymentEntity) => 
                        <DebtPayment key={payment.id} debtPayment={payment}
                            onEditCallback={onDebtPaymentUpdated} 
                            onDeleteCallback={onDebtPaymentDeleted}/>
                    )
                }
            </SimpleGrid>
        </Box>
    )
    
}

export default DebtsPage;