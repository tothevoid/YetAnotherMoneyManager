import './App.css';
import Header from './components/structure/Header/Header';
import TransactionsPage from './pages/TransactionsPage/TransactionsPage'
import Footer from './components/structure/Footer/Footer';
import DepositsPage from './pages/DepositsPage/DepositsPage';

import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import AccountsPage from './pages/AccountsPage/AccountsPage';
import DataPage from './pages/DataPage/DataPage';
import BrokerAccountsPage from './pages/BrokerAccountsPage/BrokerAccountsPage';
import SecuritiesPage from './pages/SecuritiesPage/SecuritiesPage';
import BrokerAccountPage from './pages/BrokerAccountPage/BrokerAccountPage';

const App = () => {
  return (
    <div className="app">
      <Header/>
      <main className="app-main">
        <Router>
          <Routes>
            <Route path="/" element={<AccountsPage /> } />
            <Route path="/transactions" element={<TransactionsPage />} />
            <Route path="/deposits" element={<DepositsPage />} />
            <Route path="/broker_accounts" element={<BrokerAccountsPage />} />
            <Route path="/securities" element={<SecuritiesPage />} />
            <Route path="/data" element={<DataPage />} />
            <Route path="/data/:tab" element={<DataPage />} />
            <Route path="/broker_account/:brokerAccountId" element={<BrokerAccountPage />} />
          </Routes>
        </Router>
      </main>
      <Footer/>
    </div>
  );
}

export default App;
