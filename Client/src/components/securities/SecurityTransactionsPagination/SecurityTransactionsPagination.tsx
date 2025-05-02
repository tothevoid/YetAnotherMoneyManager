import React, { Fragment, useEffect, useState } from 'react';

import { ButtonGroup, IconButton, Pagination } from '@chakra-ui/react';
import { LuChevronLeft, LuChevronRight } from "react-icons/lu"
import { gerSecurityTransactionsPagination } from '../../../api/securities/securityTransactionApi';

interface Props {
    brokerAccountId: string,
    onPageChanged: (pageSize: number, currentPage: number) => void 
}

interface State {
    pageSize: number,
    pagesQuantity: number,
    currentPage: number
}

const SecurityTransactionsPagination: React.FC<Props> = (props) => {
    const [state, setState] = useState<State>({ pageSize: -1, pagesQuantity: -1, currentPage: -1 })

    useEffect(() => {
        const initData = async () => {
            await requestPaginationConfig();
        }
        initData();
    }, []);

    const requestPaginationConfig = async () => {
        const paginationConfig = await gerSecurityTransactionsPagination(props.brokerAccountId);

        if (!paginationConfig) {
            return;
        }

        setState((currentState) => {
            return {...currentState, currentPage: 1, 
                pageSize: paginationConfig.pageSize, 
                pagesQuantity: paginationConfig.pagesQuantity 
            }
        })
        props.onPageChanged(paginationConfig.pageSize, 0)
    };

    const { pageSize, pagesQuantity } = state;

    if (pagesQuantity <= 1){
        return <Fragment/>
    }

    return (
        <Pagination.Root count={pagesQuantity} pageSize={pageSize} defaultPage={1}>
            <ButtonGroup variant="ghost" size="lg">
            <Pagination.PrevTrigger asChild>
                <IconButton color="text_primary">
                    <LuChevronLeft />
                </IconButton>
            </Pagination.PrevTrigger>
            <Pagination.Context>
            {
                ({ pages }) => pages.map((page, index) =>
                    page.type === "page" ? 
                        <Pagination.Item color="text_primary" key={index} {...page}>{page.value}</Pagination.Item>: 
                        <Pagination.Ellipsis key={index} index={index} />
                )
            }
            </Pagination.Context>
    
            <Pagination.NextTrigger asChild>
                <IconButton color="text_primary">
                    <LuChevronRight />
                </IconButton>
            </Pagination.NextTrigger>
            </ButtonGroup>
      </Pagination.Root>
    );
}

export default SecurityTransactionsPagination;