import './App.css';
import Header from './components/Header/Header';
import Manager from './pages/Manager/Manager'
import Footer from './components/Footer/Footer';

const App = () => {
  return (
    <div className="app">
      <Header/>
      <main className="app-main">
        <Manager></Manager>
      </main>
      <Footer/>
    </div>
  );
}

export default App;
