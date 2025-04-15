Feature: Test Chat

    Scenario: TC-030 Customer can access chat via mail
        Given I have received a mail with a chat link
        When I click on the link with token "9ce82c4e-d015-488f-b305-69a9ec22c3d0"
        Then I am redirected to a unique chat
         
    Scenario: TC-031 Customer can write a message in the chatt
        Given I am on the customer chat-page
        When I input a message in the textarea 
            """
            I would like to get some help now!
            """
        And click the button "Send message..."
        And refresh the page
        Then I will see the message with a timestamp in the chat window

    @ignore
    Scenario: TC-032 Customer support can view the same chat
        Given I am on the My Cases view
        And I see the list with my cases
        When I click on the case with id "40"
        Then I will se the same chat as the customer "Peter" sent

    @ignore
    Scenario: TC-033 Customer and customer support can send messages to each other
        Given the customer have a open chat
        And the customer service have a open chat
        When the customer sends a message
        And the customer support refreshes the page
        And the customer support see a message from the customer
        And sends a reply message
        Then the customer will see the new message
    @ignore
    Scenario: TC-034 Customer support can close the case and the chat
        Given the customer support is on the same chat
        When the customer support clicks on close case
        Then the case will change status in the My Cases list