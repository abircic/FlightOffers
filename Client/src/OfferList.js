import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { format } from 'date-fns';
import './OfferList.css'; // Dodajte CSS datoteku za prilagodbu stila

function OfferList({ offers, requestData }) {
  if (!offers || offers.length === 0) {
    return <p>No flight offers available.</p>;
  }
  return (
    <div className="mt-4">
      <h2>Flight Offers List</h2>
      <table className="table table-bordered">
        <thead>
          <tr>
            <th>Origin</th>
            <th>Destination</th>
            <th className="small-header">Outbound Transfers</th>
            <th className="small-header">Outbound Transfers Info</th>
            <th className="small-header">Inbound Transfers</th>
            <th className="small-header">Inbound Transfers Info</th>
            <th>Departure Date</th>
            <th>Return Date</th>
            <th>Total Price</th>
            <th>Currency Code</th>
            <th>Adults</th>

          </tr>
        </thead>
        <tbody>
          {offers.map((offer) => (
            <tr key={offer.id}>
              <td>{requestData.originLocationCode}</td>
              <td>{requestData.destinationLocationCode}</td>
              <td>{offer.numberOfOutBoundTransfers}</td>
              <td>
                <ul>
                  {offer.outBoundTransfers.map((transfer, index) => (
                    <li key={index}>
                      <strong>Departure:</strong> {transfer.departure.iataCode} at {format(new Date(transfer.departure.at), 'dd.MM.yyyy.hh:mm')}
                      <br />
                      <strong>Arrival:</strong> {transfer.arrival.iataCode} at {format(new Date(transfer.arrival.at), 'dd.MM.yyyy.hh:mm')}
                    </li>
                  ))}
                </ul>
              </td>
              <td>{offer.numberOfInBoundTransfers}</td>
              <td>
                {offer.inBoundTransfers ? (
                  <ul>
                    {offer.inBoundTransfers.map((transfer, index) => (
                      <li key={index}>
                        <strong>Departure:</strong> {transfer.departure.iataCode} at {format(new Date(transfer.departure.at), 'dd.MM.yyyy.hh:mm')}
                        <br />
                        <strong>Arrival:</strong> {transfer.arrival.iataCode} at {format(new Date(transfer.arrival.at), 'dd.MM.yyyy.hh:mm')}
                      </li>
                    ))}
                  </ul>
                ) : (
                  <p>No inbound transfers</p>
                )}
              </td>
              <td>{requestData.departureDate}</td>
              <td>{requestData.returnDate}</td>
              <td>{offer.totalPrice}</td>
              <td>{requestData.currencyCode}</td>
              <td>{requestData.adults}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default OfferList;
