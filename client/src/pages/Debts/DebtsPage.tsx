import { Box} from "@chakra-ui/react";
import DebtsList from "./components/DebtsList/DebtsList";
import DebtsPaymentsList from "./components/DebtsPaymentsList/DebtsPaymentsList";
import { useState } from "react";

const DebtsPage: React.FC = () => {
	const [hasDebts, setHasDebts] = useState(false);

	const onDebtsChanged = (quantity: number) => {
		const nowHasDebts = quantity > 0;
		
		if (nowHasDebts !== hasDebts) {
			setHasDebts(nowHasDebts);
		}
	}

	return (
		<Box>
			<DebtsList onDebtsChanged={onDebtsChanged}/>
			{hasDebts && <DebtsPaymentsList/>}
		</Box>
	)
}

export default DebtsPage;