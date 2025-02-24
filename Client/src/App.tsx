import './App.css';
import Header from './components/Header/Header';
import Manager from './pages/Manager/Manager'
import Footer from './components/Footer/Footer';
import DepositsPage from './pages/DepositsPage/DepositsPage';

import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';

const App = () => {
  return (
    <div className="app">
      <Header/>
      <main className="app-main">
        <Router>
          <Routes>
            <Route path="/" element={<Manager />} />
            <Route path="/deposits" element={<DepositsPage />} />
          </Routes>
        </Router>
      </main>
      <Footer/>
    </div>
  );
}

export default App;
