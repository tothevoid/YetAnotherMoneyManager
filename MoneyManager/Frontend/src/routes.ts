import React from 'react';
import Manager from './pages/Manager/Manager';
import App from './App';

export default [
    {
        path: '/manager',
        ...App,
        routes: [
            {
                ...Manager
            }
        ]
    },
];