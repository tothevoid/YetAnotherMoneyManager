import React from 'react';
import './App.css';
import Header from './components/Header/Header';
import Transactions from './pages/Transactions/Transactions'
import Footer from './components/Footer/Footer';

const App = () => {
  return (
    <div className="app">
      <Header/>
      <main className="app-main">
        <Transactions></Transactions>
      </main>
      <Footer/>
    </div>
  );
}

export default App;
