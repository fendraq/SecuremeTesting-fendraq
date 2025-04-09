Feature: Test New Customer Case

    Scenario: Adding a new customer case
        Given I am on the Webshop page
        When I click on the Add new case-icon
        And I see a form with input fields
        And I choose Shipping category
        And I type "fendraq@gmail.com" in the E-postadress field
        And I type "Peter" in the Förnamn field
        And I type "Svensson" in the Efternamn field
        And I type "Leveransproblem" in the Rubrik field
        And I type "Min leverans är 5 veckor sen" in Beskriv ditt ärende field
        And I click on the Send button
        Then I will get an alert saying "Message sent successfully"