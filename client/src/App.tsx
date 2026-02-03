import './App.scss';
import Header from '../features/Navigation/Header/Header';
import TransactionsPage from './pages/Transactions/TransactionsPage'
import DepositsPage from './pages/Deposits/DepositsPage';

import { BrowserRouter as Router, Route, Routes, Outlet, Navigate } from 'react-router-dom';
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
import CryptoAccountPage from './pages/CryptoAccountPage/CryptoAccountPage';
import AuthPage from './pages/Auth/AuthPage';
import { UserProvider } from '../features/UserProfileSettingsModal/hooks/UserProfileContext.tsx';
import CashAccountPage from './pages/CashAccountPage/CashAccountPage.tsx';


const RequireAuth = () => {
	const token = localStorage.getItem("auth_token");
	return token ? <Outlet /> : <Navigate to="/auth" replace state={{ from: location.pathname }}/>;
}

const PageWrapper = () => (
	<UserProvider>
		<div className="app">
			<div className="header">
					<Header />
				</div>
				<main className="app-main">
					<Outlet />
				</main>
		</div>
	</UserProvider>
);

const App = () => {
	return (
		<Router>
			<Routes>
				<Route path="/auth" element={<AuthPage />} />
				<Route element={<PageWrapper />}>
					<Route element={<RequireAuth />}>
						<Route path="/" element={<DashboardPage />} />	
						<Route path="/accounts" element={<AccountsPage /> } />
						<Route path="/transactions" element={<TransactionsPage />} />
						<Route path="/deposits" element={<DepositsPage />} />
						<Route path="/broker_accounts" element={<BrokerAccountsPage />} />
						<Route path="/securities" element={<SecuritiesPage />} />
						<Route path="/debts" element={<DebtsPage />} />
						<Route path="/cryptocurrencies" element={<CryptocurrenciesPage />} />
						<Route path="/crypto_accounts" element={<CryptoAccountsPage />} />
						<Route path="/crypto_account/:cryptoAccountId" element={<CryptoAccountPage />} />
						<Route path="/data" element={<DataPage />} />
						<Route path="/data/:tab" element={<DataPage />} />
						<Route path="/broker_account/:brokerAccountId" element={<BrokerAccountPage />} />
						<Route path="/security/:securityId" element={<SecurityPage />} />
						<Route path="/cash_account/:cashAccountId" element={<CashAccountPage />} />
					</Route>
				</Route>
			</Routes>
		</Router>
	);
}

export default App;
