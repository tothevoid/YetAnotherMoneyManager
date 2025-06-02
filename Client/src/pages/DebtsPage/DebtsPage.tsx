import { useEffect, useState } from "react";
import { SimpleGrid, Box} from "@chakra-ui/react";
import { useTranslation } from "react-i18next";
import { ClientDebtEntity } from "../../models/debts/DebtEntity";
import { createDebt, getDebts, updateDebt } from "../../api/debts/debtApi";
import AddDebtButton from "../../components/debts/AddDebtButton/AddDebtButton";
import AddDebtPaymentButton from "../../components/debts/AddDebtPaymentButton/AddDebtPaymentButton";
import { ClientDebtPaymentEntity } from "../../models/debts/DebtPaymentEntity";
import { createDebtPayment, getDebtPayments, updateDebtPayment } from "../../api/debts/debtPaymentApi";
import Debt from "../../components/debts/Debt/Debt";

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

    
    const onDeptPaymentUpdated = async (debtPayment: ClientDebtPaymentEntity) => {
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

    const onDeptPaymentDeleted = (deletedDebtPayment: ClientDebtPaymentEntity) => {
        const debtPayments = state.debtPayments
            .filter(debtPayment => debtPayment.id !== deletedDebtPayment.id);
        setState((currentState) => {
            return {...currentState, debtPayments};
        });
    }

    return (
        <Box paddingBlock={10}>
            <AddDebtButton onAdded={onDebtAdded}/>
            <SimpleGrid pt={5} pb={5} gap={6} templateColumns='repeat(auto-fill, minmax(300px, 4fr))'>
                {
                    state.debts.map((debt: ClientDebtEntity) => 
                        <Debt key={debt.id} debt={debt} 
                            onEditCallback={onDeptUpdated}
                            onDeleteCallback={onDeptDeleted}/>
                    )
                }
            </SimpleGrid>
            <AddDebtPaymentButton onAdded={onDebtPaymentAdded}/>
            <SimpleGrid pt={5} pb={5} gap={6} templateColumns='repeat(auto-fill, minmax(300px, 4fr))'>
                {
                    state.debts.map((debt: ClientDebtEntity) => 
                        <DebtPayment key={deposit.id} deposit={deposit} 
                            onUpdated={onDeptUpdated} 
                            onCloned={onDebtAdded} 
                            onDeleted={onDeptDeleted}/>
                    )
                }
            </SimpleGrid>
        </Box>
    )
    
}

export default DebtsPage;