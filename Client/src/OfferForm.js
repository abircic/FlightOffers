import React, { useState } from 'react';
import axios from 'axios';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import { format } from 'date-fns';
import './OfferForm.css';

function OfferForm({ onFetchOffers, onFetchError, onFormSubmit }) {
  const [originLocationCode, setOriginLocationCode] = useState('');
  const [destinationLocationCode, setDestinationLocationCode] = useState('');
  const [departureDate, setDepartureDate] = useState(null);
  const [returnDate, setReturnDate] = useState(null);
  const [adults, setAdults] = useState(1);
  const [currencyCode, setCurrencyCode] = useState('EUR');
  const [loading, setLoading] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true)
    if (originLocationCode.length !== 3 || destinationLocationCode.length !== 3) {
        onFetchError('Origin and Destination Location Codes must be 3 characters long.');
        return;
      }
    try {
        const apiUrl = `http://localhost:5000/api/public/offer/Fetch?OriginLocationCode=${originLocationCode}&DestinationLocationCode=${destinationLocationCode}&DepartureDate=${format(new Date(departureDate), 'yyyy-MM-dd')}&Adults=${adults}&ReturnDate=${returnDate ? format(new Date(returnDate), 'yyyy-MM-dd') : ''}&CurrencyCode=${currencyCode}`;
        const response = await axios.get(apiUrl);
        setLoading(false)
        onFetchOffers(response.data.offers);
        onFormSubmit({
          originLocationCode,
          destinationLocationCode,
          departureDate: format(new Date(departureDate), 'dd.MM.yyyy.'),
          returnDate: returnDate ? format(new Date(returnDate), 'dd.MM.yyyy.') : 'N/A',
          adults,
          currencyCode
        });
    } catch (error) {
      setLoading(false)
        if (error.response && error.response.data && error.response.data.message) {
          onFetchError(error.response.data.message);
        } else {
          onFetchError('API is currently unavailable. Please try again later.');
        }
      }
    };
  

  return (
    <div className="form-container"> 
    <form onSubmit={handleSubmit}>
      <div className="form-group">
        <label htmlFor="origin">Origin location code:</label>
        <input
          type="text"
          className="form-control"
          id="origin"
          value={originLocationCode}
          onChange={(e) => setOriginLocationCode(e.target.value)}
          required
          placeholder ='Example: SYD'
        />
      </div>
      <div className="form-group">
        <label htmlFor="destination">Destination location code:</label>
        <input
          type="text"
          className="form-control"
          id="destination"
          value={destinationLocationCode}
          onChange={(e) => setDestinationLocationCode(e.target.value)}
          required
          placeholder ='Example: BKK'
        />
      </div>
      <div className="form-group">
        <label htmlFor="departure">Departure date:</label>
        <br />
        <DatePicker
          selected={departureDate}
          onChange={(date) => setDepartureDate(date)}
          dateFormat="yyyy-MM-dd"
          className="form-control"
          id="departure"
          required
        />
      </div>
      <div className="form-group">
        <label htmlFor="return">Return date:</label>
        <br />
        <DatePicker
          selected={returnDate}
          onChange={(date) => setReturnDate(date)}
          dateFormat="yyyy-MM-dd"
          className="form-control"
          id="return"
          placeholderText='Optional'
        />
      </div>
      <div className="form-group">
        <label htmlFor="adults">Number of adults:</label>
        <input
          type="number"
          className="form-control"
          id="adults"
          value={adults}
          onChange={(e) => setAdults(e.target.value)}
          min="1"
          max="10"
          required
        />
      </div>
      <div className="form-group">
        <label htmlFor="currency">Currency:</label>
        <select
          className="form-control"
          id="currency"
          value={currencyCode}
          onChange={(e) => setCurrencyCode(e.target.value)}
          required
        >
          <option value="EUR">EUR</option>
          <option value="HRK">HRK</option>
          <option value="USD">USD</option>
        </select>
      </div>
      {loading ? (
          <p>Loading...</p>
        ) : (
          <button type="submit" className="btn btn-primary">
            Fetch
          </button>
        )}
    </form>
    </div>

  );
}

export default OfferForm;
