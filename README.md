# CustomerRecognitionService

## Contents

Endpoint to get merged customer history /CustomerRecognition/GetMergedCustomerHistory

Endpooint to save customer /CustomerRecognition/SaveCustomer
Example data :
{
  "firstName": "Pirmas",
  "lastName": "Testas",
  "email": "pirmas2777@testas.com",
  "phoneNumber": "+37033166632",
  "address": "Ok"
}

If duplicate customer is found when saving - a pending merge is created.

Background to check pending merges runs every 5 minutes. 
Then executes merge to update master record with the newest data while preserving new customer data in a new row.