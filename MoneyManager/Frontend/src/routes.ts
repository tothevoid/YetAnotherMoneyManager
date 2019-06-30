import React from 'react';
import Transactions from './pages/Transactions/Transactions';
import App from './App';

export default [
    {
        path: '/transactions',
        ...App,
        routes: [
            {
                ...Transactions
            }
        ]
    },
];