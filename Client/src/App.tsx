import './App.css';
import Header from '../features/Navigation/Header/Header';
import TransactionsPage from './pages/Transactions/TransactionsPage'
import DepositsPage from './pages/Deposits/DepositsPage';

import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import AccountsPage from './pages/Accounts/AccountsPage';
import DataPage from './pages/Data/DataPage';
import BrokerAccountsPage from './pages/BrokerAccounts/BrokerAccountsPage';
import SecuritiesPage from './pages/SecuritiesPage/SecuritiesPage';
import BrokerAccountPage from './pages/BrokerAccount/BrokerAccountPage';
import SecurityPage from './pages/SecurityPage/SecurityPage';
import DashboardPage from './pages/Dashboard/DashboardPage';
import DebtsPage from './pages/Debts/DebtsPage';
import CryptocurrenciesPage from './pages/CryptocurrenciesPage/CryptocurrenciesPage';
import CryptoAccountsPage from './pages/CryptoAccountsPage/CryptoAccountsPage';

const App = () => {
  return (
    <div className="app">  
        <Router>
          <div className="header">
            <Header/>
          </div>
          <main className="app-main">
            <Routes>
                <Route path="/" element={<DashboardPage />} />  
                <Route path="/accounts" element={<AccountsPage /> } />
                <Route path="/transactions" element={<TransactionsPage />} />
                <Route path="/deposits" element={<DepositsPage />} />
                <Route path="/broker_accounts" element={<BrokerAccountsPage />} />
                <Route path="/securities" element={<SecuritiesPage />} />
                <Route path="/debts" element={<DebtsPage />} />
                <Route path="/cryptocurrencies" element={<CryptocurrenciesPage />} />
                <Route path="/crypto_accounts" element={<CryptoAccountsPage />} />
                <Route path="/data" element={<DataPage />} />
                <Route path="/data/:tab" element={<DataPage />} />
                <Route path="/broker_account/:brokerAccountId" element={<BrokerAccountPage />} />
                <Route path="/security/:securityId" element={<SecurityPage />} />
              </Routes>
          </main>
         </Router>
    </div>
  );
}

export default App;
