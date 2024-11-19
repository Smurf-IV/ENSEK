# Ensek Remote Technical Test
- To demonstrate a Web API that allows the upload of a customers `Meter Reading CSV` file


## Must Have:
- Create the following endpoint:
  - `POST => /meter-reading-uploads`

- The endpoint should be able to process a CSV of meter readings.
  - An example CSV file has been provided (Meter_reading.csv)

- Each entry in the CSV should be validated and if valid, stored in a DB.
  - **Q:** What should be done when the values (i.e. dateColumn) are not in the correct format ?
  - **A:** See Next, A record of failed reading should be kept, and returned.

- After processing, the number of successful/failed readings should be returned.


## Validation: 
- You should not be able to load the same entry twice
  - **Q:** Should an error be returned 
  - **A:** See Above, A record of failed reading should be kept, and returned.

- A meter reading must be associated with an Account ID to be deemed valid
  - **Q:** What should be done with the invalid account reference data ?
  - **A:** See Above, A record of failed reading should be kept, and returned.

- Reading values should be in the format NNNNN
  - **Q:** What should be done when the values are not in this format ?
  - **A:** See Above, A record of failed reading should be kept, and returned.


## Nice To Have:
- Create a client in the technology of your choosing to consume the API. 
  - You can use angular/react/whatever you like

- When an account has an existing read, ensure the new read isn’t older than the existing read

