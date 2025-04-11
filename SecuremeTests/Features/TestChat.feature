Feature: Test Chat

    Scenario: TC-030 Customer can access chat via mail
        Given I have received a mail with a chat link
        When I click on the link with token "ad526171-df60-4389-982c-c29af74f9dc9"
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

    Scenario: Customer support can view the same chat

    Scenario: Customer and customer support can send messages to each other

    Scenario: Customer support can close the case and the chat