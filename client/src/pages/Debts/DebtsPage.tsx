import { Box} from "@chakra-ui/react";
import DebtsList from "./components/DebtsList/DebtsList";
import DebtsPaymentsList from "./components/DebtsPaymentsList/DebtsPaymentsList";

const DebtsPage: React.FC = () => {
	return (
		<Box>
			<DebtsList/>
			<DebtsPaymentsList/>
		</Box>
	)
}

export default DebtsPage;