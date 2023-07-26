import React, { useState } from 'react';
import OfferForm from './OfferForm';
import OfferList from './OfferList';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

function App() {
  const [offers, setOffers] = useState([]);
  const [error, setError] = useState('');
  const [submit, setSubmit] = useState(false); 
  const [requestData, setRequestData] = useState({});

  const handleFetchOffers = (data) => {
    setOffers(data);
    setError('');
    setSubmit(true);
  };

  const handleFetchError = (errorMessage) => {
    setError(errorMessage);
    setOffers([]);
  };

  const handleFormSubmit = (data) => {
    setRequestData(data);
  };
 return (
    <div className="container mt-5">
      <h1>Flight Offers</h1>
      <OfferForm onFetchOffers={handleFetchOffers} onFetchError={handleFetchError} onFormSubmit={handleFormSubmit} />
      {error && <div className="alert alert-danger mt-3">{error}</div>}
      {submit && <OfferList offers={offers} requestData={requestData} />}
    </div>
  );
}

export default App;
