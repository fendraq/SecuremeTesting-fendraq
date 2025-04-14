Feature: Test New Customer Case

    Scenario: TC-027 Adding a new customer case with valid data
        Given I am on the Webshop page
        When I click on the Add new case-icon
        And I see a form with input fields
        And I choose Shipping category
        And I type "fendraq@gmail.com" in the E-postadress field
        And I type "Peter" in the Förnamn field
        And I type "Svensson" in the Efternamn field
        And I type "Leveransproblem" in the Rubrik field
        And I type "Min leverans är 5 veckor sen" in Beskriv ditt ärende field and click send
        Then I will get an alert saying "Message sent successfully"

    @ignore
    Scenario: TC-028 Filling a new customer case with wrong email format
        Given I am on the Webshop page
        When I click on the Add new case-icon
        And I see a form with input fields
        And I choose Shipping category
        And I type an invalid email "fendraqgmail.com" in the E-postadress field
        And I type "Peter" in the Förnamn field
        And I type "Svensson" in the Efternamn field
        And I type "Leveransproblem" in the Rubrik field
        And I type "Min leverans är 5 veckor sen" in Beskriv ditt ärende field and click send
        Then I will get an alert error saying "Email is in the wrong format"
        
    Scenario: TC-029 Leaving a field empty when adding a new case
        Given I am on the Webshop page
        When I click on the Add new case-icon
        And I see a form with input fields
        And I choose Shipping category
        And I type an invalid email "fendraqgmail.com" in the E-postadress field
        And I type "Peter" in the Förnamn field
        And I type "Svensson" in the Efternamn field
        And I type "Min leverans är 5 veckor sen" in Beskriv ditt ärende field and click send
        Then I will se an alert error saying "Alla fält är obligatoriska!"    