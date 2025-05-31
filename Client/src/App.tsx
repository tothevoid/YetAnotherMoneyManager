import './App.css';
import Header from './components/structure/Header/Header';
import TransactionsPage from './pages/TransactionsPage/TransactionsPage'
import DepositsPage from './pages/DepositsPage/DepositsPage';

import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import AccountsPage from './pages/AccountsPage/AccountsPage';
import DataPage from './pages/DataPage/DataPage';
import BrokerAccountsPage from './pages/BrokerAccountsPage/BrokerAccountsPage';
import SecuritiesPage from './pages/SecuritiesPage/SecuritiesPage';
import BrokerAccountPage from './pages/BrokerAccountPage/BrokerAccountPage';
import SecurityPage from './pages/SecurityPage/SecurityPage';
import DashboardPage from './pages/DashboardPage/DashboardPage';
import DebtsPage from './pages/DebtsPage/DebtsPage';

const App = () => {
  return (
    <div className="app">
      <div className="header">
        <Header/>
      </div>
      <main className="app-main">
        <Router>
          <Routes>
            <Route path="/" element={<DashboardPage />} />  
            <Route path="/accounts" element={<AccountsPage /> } />
            <Route path="/transactions" element={<TransactionsPage />} />
            <Route path="/deposits" element={<DepositsPage />} />
            <Route path="/broker_accounts" element={<BrokerAccountsPage />} />
            <Route path="/securities" element={<SecuritiesPage />} />
            <Route path="/debts" element={<DebtsPage />} />
            <Route path="/data" element={<DataPage />} />
            <Route path="/data/:tab" element={<DataPage />} />
            <Route path="/broker_account/:brokerAccountId" element={<BrokerAccountPage />} />
            <Route path="/security/:securityId" element={<SecurityPage />} />
          </Routes>
        </Router>
      </main>
    </div>
  );
}

export default App;
