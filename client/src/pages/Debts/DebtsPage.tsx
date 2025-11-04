import { Box} from "@chakra-ui/react";
import DebtsList from "./components/DebtsList/DebtsList";
import DebtsPaymentsList from "./components/DebtsPaymentsList/DebtsPaymentsList";
import { useState } from "react";

const DebtsPage: React.FC = () => {
	const [hasDebts, setHasDebts] = useState(false);

	const [version, setVersion] = useState(-1);

	const onDebtsChanged = (quantity: number) => {
		const nowHasDebts = quantity > 0;
		
		if (nowHasDebts !== hasDebts) {
			setHasDebts(nowHasDebts);
		}
	}

	const onDebtPaymentsChanged = () => {
		setVersion(prev => prev + 1);
	}

	return (
		<Box>
			<DebtsList debtsPaymentsVersion={version} onDebtsChanged={onDebtsChanged}/>
			{hasDebts && <DebtsPaymentsList onDebtPaymentsChanged={onDebtPaymentsChanged}/>}
		</Box>
	)
}

export default DebtsPage;