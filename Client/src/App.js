import React, { useState } from 'react';
import OfferForm from './OfferForm';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

function App() {
  const [offers, setOffers] = useState([]);
  const [error, setError] = useState('');
  const [requestData, setRequestData] = useState({});

  const handleFetchOffers = (data) => {
    setOffers(data);
    setError('');
  };

  const handleFetchError = (errorMessage) => {
    setError(errorMessage);
    setOffers([]);
  };

 return (
    <div className="container mt-5">
      <h1>Flight Offers</h1>
      <OfferForm onFetchOffers={handleFetchOffers} onFetchError={handleFetchError} />
      {error && <div className="alert alert-danger mt-3">{error}</div>}
    </div>
  );
}

export default App;
