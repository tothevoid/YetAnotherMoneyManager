import './App.css';
import Header from './components/Header/Header';
import TransactionsPage from './pages/TransactionsPage/TransactionsPage'
import Footer from './components/Footer/Footer';
import DepositsPage from './pages/DepositsPage/DepositsPage';

import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import AccountsPage from './pages/AccountsPage/AccountsPage';
import DataPage from './pages/DataPage/DataPage';

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
            <Route path="/data" element={<DataPage />} />
            <Route path="/data/:tab" element={<DataPage />} />
          </Routes>
        </Router>
      </main>
      <Footer/>
    </div>
  );
}

export default App;
