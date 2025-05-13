import { Fragment, useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { getSecurityById } from "../../api/securities/securityApi";
import { SecurityEntity } from "../../models/securities/SecurityEntity";
import { Stack, Text} from "@chakra-ui/react";
import DividendList from "../../components/securities/DividendsList/DividendList";

interface Props {}

interface State {
	security: SecurityEntity
}

const SecurityPage: React.FC<Props> = () => {
	const { t } = useTranslation();

	const { securityId } = useParams();

	const [state, setState] = useState<State>({ security: null })

	if (!securityId) {
		return <Fragment/>
	}

	const initData = async () => {
		const security = await getSecurityById(securityId);
		if (!security) {
			return;
		}

		setState((currentState) => {
			return {...currentState, security}
		})
	}

	useEffect(() => {
		initData();
	}, []);

	if (!state.security) {
		return <Fragment/>
	}

	const { ticker, name, type, actualPrice} = state.security;

	return (
		<Stack color="text_primary">
			<Stack alignItems="center" gap={2} direction="row">
				<Text fontSize="2xl">{ticker}</Text>
				<Text>({name})</Text>
			</Stack>
			<Text>{type.name}</Text>
			<Text>{actualPrice}</Text>
			<DividendList securityId={securityId}/>
		</Stack>
	)
}

export default SecurityPage;
