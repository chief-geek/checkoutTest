# checkoutTest
Test for checkout

The code can be built and all the tests pass

I have covered pretty much every bit of functional code in the project, you can simply execute the tests.

However, if you wish to check that it works

Steps
  publish the database project so that it creates the database. 
  Update the connection string in appsettings.json file
  click run, both api's should execute
  localhost:7050 is the public facing api, use the acceptpayment and then use the get transaction

what I would have done differently

I would have encrypted the entire payload before sending to the gateway
decrypt the payload and do the work to ensure further security
I would have liked to add fraud prevention, but time is a factor and I need to research that
