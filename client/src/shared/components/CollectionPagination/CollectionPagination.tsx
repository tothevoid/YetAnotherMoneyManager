import React, { Fragment, useEffect, useState } from 'react';

import { ButtonGroup, Flex, IconButton, Pagination } from '@chakra-ui/react';
import { LuChevronLeft, LuChevronRight } from "react-icons/lu"
import { PaginationConfig } from '../../models/PaginationConfig';

interface Props {
	getPaginationConfig: () => Promise<PaginationConfig | void>,
	onPageChanged: (pageSize: number, currentPage: number) => void 
}

interface State {
	pageSize: number,
	recordsQuantity: number,
}

const CollectionPagination: React.FC<Props> = ({ getPaginationConfig, onPageChanged }) => {
	const [state, setState] = useState<State>({ pageSize: -1, recordsQuantity: -1})

	useEffect(() => {
		const initData = async () => {
			await requestPaginationConfig();
		}
		initData();
	}, []);

	const requestPaginationConfig = async () => {
		const paginationConfig = await getPaginationConfig();

		if (!paginationConfig) {
			return;
		}

		setState((currentState) => {
			return {...currentState, 
				pageSize: paginationConfig.pageSize, 
				recordsQuantity: paginationConfig.recordsQuantity 
			}
		})
		onPageChanged(paginationConfig.pageSize, 0)
	};

	const onPageChange = (page: number, pageSize: number) => {
		onPageChanged(pageSize, page);
	}

	const { pageSize, recordsQuantity } = state;

	if (recordsQuantity <= 1){
		return <Fragment/>
	}

	return (
		<Flex justifyContent={"center"}>
			<Pagination.Root onPageChange={({page, pageSize}) => onPageChange(page, pageSize)} 
				count={recordsQuantity} pageSize={pageSize} defaultPage={1}>
				<ButtonGroup variant="ghost" size="lg">
					<Pagination.PrevTrigger asChild>
						<IconButton color="text_primary">
							<LuChevronLeft />
						</IconButton>
					</Pagination.PrevTrigger>
					<Pagination.Items
						render={(page) => (
						<IconButton color="text_primary" variant={{ base: "ghost", _selected: "outline" }}>
						{page.value}
						</IconButton>
					)}
					/>
					<Pagination.NextTrigger asChild>
						<IconButton color="text_primary">
							<LuChevronRight />
						</IconButton>
					</Pagination.NextTrigger>
				</ButtonGroup>
	  		</Pagination.Root>	
		</Flex>
		
	);
}

export default CollectionPagination;